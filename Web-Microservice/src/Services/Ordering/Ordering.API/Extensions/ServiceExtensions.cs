using Infrastructure.Configuation;
using Infrastructure.Extensions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ordering.API.Applications.IntegrationEvents.Consumers;
using Shared.Configurations;

namespace Ordering.API.Extensions
{
    public static class ServiceExtensions
    {
        internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var emailSetting = configuration.GetSection(nameof(SMTPEmailSetting))
                                .Get<SMTPEmailSetting>();

            services.AddSingleton(emailSetting);
            return services;
        }

        public static void ConfigureMasstransit(this IServiceCollection services)
        {
            var settings = services.GetOptions<EventBusSettings>("EventBusSettings");
            if (settings == null || string.IsNullOrEmpty(settings.HostAddress))
            {
                throw new ArgumentNullException("event bus connectionstring is empty!!");
            }

            var mqConnection = new Uri(settings.HostAddress);
            services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
            services.AddMassTransit(config => 
            {
                config.AddActivitiesFromNamespaceContaining<BasketCheckoutConsumer>();
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(mqConnection);
                    cfg.ReceiveEndpoint("basket-checkout-queue", c =>
                    {
                        c.ConfigureConsumer<BasketCheckoutConsumer>(ctx);
                    });
                    //cfg.ConfigureEndpoints(ctx);
                });
            });
        }

    }
}
