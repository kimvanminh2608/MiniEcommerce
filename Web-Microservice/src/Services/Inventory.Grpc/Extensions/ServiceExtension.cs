using Infrastructure.Extensions;
using Inventory.Grpc.Repositories;
using Inventory.Grpc.Repositories.Interfaces;
using MongoDB.Driver;
using Shared.Configurations;

namespace Inventory.Grpc.Extensions
{
    public static class ServiceExtension
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            //services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));
            services.AddScoped<IInventoryRepository, InventoryRepository>();
        }

        internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var dataSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
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
            var settings = services.GetOptions<MongoDbSettings>(nameof(MongoDbSettings));
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
