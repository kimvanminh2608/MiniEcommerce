using Ocelot.DependencyInjection;
using Shared.Configurations;

namespace OcelotApiGw.Extensions
{
    public static class ServiceExtension
    {
        internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            //var eventBusSetting = configuration.GetSection(nameof(EventBusSettings))
            //                    .Get<EventBusSettings>();

            //services.AddSingleton(eventBusSetting);

            //var cacheSetting = configuration.GetSection(nameof(CacheSettings))
            //                    .Get<CacheSettings>();

            //services.AddSingleton(cacheSetting);

            //var grpcSetting = configuration.GetSection(nameof(GrpcSetting))
            //                    .Get<GrpcSetting>();

            //services.AddSingleton(grpcSetting);
            return services;
        }

        public static void ConfigureOcelot(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOcelot(configuration);
        }

        public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
        {
            var origins = configuration["AllowOrigins"];
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder =>
                    {
                        builder.WithOrigins(origins)
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });
        }
    }
}
