using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        private readonly ILogger _logger;
        private readonly OrderContext _context;

        public OrderContextSeed(ILogger logger, OrderContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task InitializeAsync()
        {
            try
            {
                if (_context.Database.IsSqlServer())
                {
                    await _context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("an error has occured when initializing database");
                throw ex;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                if (!_context.Orders.Any())
                {
                    await _context.Orders.AddRangeAsync(new Order
                    {
                        UserName = "Customer1",
                        FirstName = "customer 1",
                        LastName = "customer 1",
                        EmailAddress = "abc@gmail.com",
                        ShippingAddress = "gp vap",
                        InvoiceAddress = "VN",
                        TotalPrice = 1000
                    });
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                _logger.Error("An error occured when seeding database");
                throw;
            }
        }
    }
}
