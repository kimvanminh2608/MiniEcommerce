using Contracts.Commons.Events;
using Contracts.Commons.Interfaces;
using Infrastructure.Commons;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Extensions
{
    public static class MediatorExtension
    {
        public static async Task DispactDomainEventAsync(
            this IMediator mediator,
            List<BaseEvent> domainEvents,
            ILogger logger)
        {

            foreach (var domainEvent in domainEvents)
            {
                await mediator.Publish(domainEvent);
                var data = new SerializeService().Serialize(domainEvent);
                logger.Information($"\n----\nA domain event published!\n"+
                                    $"Event: {domainEvent.GetType().Name}\n"+
                                    $"Data: {data}\n----\n");
            }
        }
    }
}
