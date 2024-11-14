using Inventory.Product.API.Entities;
using Inventory.Product.API.Extensions;
using MongoDB.Driver;
using Shared.Configurations;
using Shared.Enums.Inventory;

namespace Inventory.Product.API.Persistences
{
    public class InventoryDbSeed
    {
        public async Task SeedDataAsync(IMongoClient client, MongoDbSettings setting)
        {
            var databaseName = setting.DatabaseName;
            var database = client.GetDatabase(databaseName);
            var inventoryCollection = database.GetCollection<InventoryEntry>("InventoryIntries");
            if (await inventoryCollection.EstimatedDocumentCountAsync() == 0)
            {
                await inventoryCollection.InsertManyAsync(GetPreconfigureInventoryEntries());
            }
        }

        private IEnumerable<InventoryEntry> GetPreconfigureInventoryEntries()
        {
            return new List<InventoryEntry>()
            {
                new()
                {
                    Quantity = 10,
                    DocumentNo = Guid.NewGuid().ToString(),
                    ItenNo = "Lotus",
                    ExternalDocumentNo = Guid.NewGuid().ToString(),
                    DocumentType = EDocumentType.Purchase,
                }
            };
        }
    }
}
