using Inventory.Product.API.Persistences;
using MongoDB.Driver;
using Shared.Configurations;

namespace Inventory.Product.API.Extensions
{
    public static class HostExtension
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var settings = services.GetService<MongoDbSettings>();
            if (settings == null || string.IsNullOrEmpty(settings.ConnectionString))
            {
                throw new ArgumentNullException("DatabaseSetting is null!");
            }
            var mongoClient = services.GetRequiredService<IMongoClient>();
            new InventoryDbSeed()
                .SeedDataAsync(mongoClient, settings)
                .Wait();

            return host;
        }
    }
}
