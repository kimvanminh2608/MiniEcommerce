using Contracts.Commons.Events;
using Contracts.Commons.Interfaces;
using Contracts.Domains.Interfaces;
using Infrastructure.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContext : DbContext
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;
        public OrderContext(DbContextOptions<OrderContext> options, IMediator mediator, ILogger logger) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public DbSet<Order> Orders { get; set; }
        private List<BaseEvent> _baseEvents;
        private void SetBaseEventBeforeSavechange()
        {
            var domainEntities = ChangeTracker.Entries<IEventEntity>()
                .Select(e => e.Entity)
                .Where(x => x.DomainEvents().Any())
                .ToList();

            _baseEvents = domainEntities.SelectMany(e => e.DomainEvents()).ToList();

            domainEntities.ForEach(e => e.ClearDomainEvent());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetBaseEventBeforeSavechange();
            var modified = ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Modified ||
                                x.State == EntityState.Added ||
                                x.State == EntityState.Deleted);

            foreach (var item in modified)
            {
                switch (item.State)
                {
                    case EntityState.Added:
                        if (item.Entity is IDateTracking addEntity)
                        {
                            addEntity.CreatedDate = DateTime.UtcNow;
                            item.State = EntityState.Added;
                        }
                        break;

                    case EntityState.Modified:
                        Entry(item.Entity).Property("Id").IsModified = false;
                        if (item.Entity is IDateTracking modifiedEntity)
                        {
                            modifiedEntity.LastModifiedDate = DateTime.UtcNow;
                            item.State = EntityState.Modified;
                        }
                        break;
                }
            }

            var result = base.SaveChangesAsync(cancellationToken);
            _mediator.DispactDomainEventAsync(_baseEvents, _logger);
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
