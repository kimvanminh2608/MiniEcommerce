using Infrastructure.Extensions;
using Inventory.Product.API.Services;
using Inventory.Product.API.Services.Interfaces;
using MongoDB.Driver;

namespace Inventory.Product.API.Extensions
{
    public static class ServiceExtension
    {
        public static void AddInfrastructureServices (this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));
            services.AddScoped<IInventoryService, InventoryService>();
        }

        internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var dataSettings = configuration.GetSection(nameof(DatabaseSetting)).Get<DatabaseSetting>();
            services.AddSingleton(dataSettings);
            return services;
        }

        public static void ConfigureMongoDbClient(this IServiceCollection services) 
        {
            services.AddSingleton<IMongoClient>(new MongoClient(GetMongoConnectionString(services)))
                .AddScoped(x => x.GetService<IMongoClient>()?.StartSession());
        }

        private static string GetMongoConnectionString(this IServiceCollection services)
        {
            var settings = services.GetOptions<DatabaseSetting>(nameof(DatabaseSetting));
            if (settings == null || string.IsNullOrEmpty(settings.ConnectionString))
            {
                throw new ArgumentNullException("DatabaseSetting is empty!");
            }

            var databaseName = settings.DatabaseName;
            var connectionString = settings.ConnectionString + "/" + databaseName + "?authSource=admin";
            return connectionString;
        }
    }
}
