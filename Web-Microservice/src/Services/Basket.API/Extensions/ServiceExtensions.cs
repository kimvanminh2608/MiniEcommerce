using Basket.API.Repositories;
using Basket.API.Repositories.Interfaces;
using Contracts.Commons.Interfaces;
using Infrastructure.Commons;

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
            services.AddInfrastructureService();
            return services;
        }

        private static IServiceCollection AddInfrastructureService(this IServiceCollection services)
        {
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddTransient<ISerializeService, SerializeService>();

            return services;
        }

        public static void ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConnectionString = configuration.GetSection("CacheSettings:ConnectionString").Value;
            if (string.IsNullOrEmpty(redisConnectionString))
            {
                throw new ArgumentNullException("Redis connectionstring is empty!!");
            }

            // Redis configuation
            services.AddStackExchangeRedisCache(options =>
            {
                // some code here
                options.Configuration = redisConnectionString;
            });
        }
    }
}
