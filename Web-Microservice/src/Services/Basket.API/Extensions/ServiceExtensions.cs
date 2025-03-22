using Basket.API.GrpcServices;
using Basket.API.Protos;
using Basket.API.Repositories;
using Basket.API.Repositories.Interfaces;
using Contracts.Commons.Interfaces;
using EventBus.Messages.IntergrationEvent.Interfaces;
using Infrastructure.Commons;
using Infrastructure.Configuation;
using Infrastructure.Extensions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shared.Configurations;
using System.Net.Security;
using System.Net.WebSockets;

namespace Basket.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection ConfigureServices (this IServiceCollection services)
        {
            // Add services to the container.

            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            // Config masstransit
            //services.ConfigureMassTransit();

            services.AddInfrastructureService();
            return services;
        }

        private static IServiceCollection AddInfrastructureService(this IServiceCollection services)
        {
            try
            {
                services.AddScoped<IBasketRepository, BasketRepository>();
                services.AddTransient<ISerializeService, SerializeService>();
                
                services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));
                return services;
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        public static void ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
        {
            try
            {
                var redisConnectionString = services.GetOptions<CacheSettings>("CacheSettings");
                if (string.IsNullOrEmpty(redisConnectionString.ConnectionString))
                {
                    throw new ArgumentNullException("Redis connectionstring is empty!!");
                }

                // Redis configuation
                services.AddStackExchangeRedisCache(options =>
                {
                    // some code here
                    options.Configuration = redisConnectionString.ConnectionString;
                });
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        public static void ConfigureMassTransit(this IServiceCollection services)
        {
            try
            {
                var settings = services.GetOptions<EventBusSettings>("EventBusSettings");
                if (settings == null || string.IsNullOrEmpty(settings.HostAddress))
                {
                    throw new ArgumentNullException("event bus connectionstring is empty!!");
                }

                var mtConnection = new Uri(settings.HostAddress);
                services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
                services.AddMassTransit(cfg =>
                {
                    cfg.UsingRabbitMq((ctx, config) =>
                    {

                        config.Host(mtConnection, h =>
                        {
                            h.Username("guest");
                            h.Password("guest");

                            h.UseSsl(s =>
                            {
                                s.UseCertificateAsAuthenticationIdentity = true;
                                //s.CertificatePath = "path/to/client_certificate.pem"; // Path to your client certificate
                                //s.CertPassphrase = "your_certificate_passphrase"; // Passphrase if needed
                                s.ServerName = "localhost"; // Server name for validation
                                //s.AcceptablePolicyErrors = SslPolicyErrors.RemoteCertificateNameMismatch | SslPolicyErrors.RemoteCertificateChainErrors;
                            });
                        });
                    });

                    cfg.AddRequestClient<IBasketCheckoutEvent>();
                });
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var eventBusSetting = configuration.GetSection(nameof(EventBusSettings))
                                .Get<EventBusSettings>();

            services.AddSingleton(eventBusSetting);

            var cacheSetting = configuration.GetSection(nameof(CacheSettings))
                                .Get<CacheSettings>();

            services.AddSingleton(cacheSetting);

            var grpcSetting = configuration.GetSection(nameof(GrpcSetting))
                                .Get<GrpcSetting>();

            services.AddSingleton(grpcSetting);
            return services;
        }

        public static IServiceCollection ConfigureGrpcServices(this IServiceCollection services)
        {
            var grpcSetting = services.GetOptions<GrpcSetting>("GrpcSetting");
            services.AddGrpcClient<StockProtoService.StockProtoServiceClient>(o =>
            {
                o.Address = new Uri(grpcSetting.StockUrl);
            });

            services.AddScoped<StockItemGrpcService>();
            return services;
        }
    }
}
