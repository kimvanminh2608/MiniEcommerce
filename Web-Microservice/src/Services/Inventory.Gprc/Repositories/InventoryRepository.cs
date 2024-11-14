using Infrastructure.Commons;
using Inventory.Gprc.Entities;
using Inventory.Gprc.Repositories.Interfaces;
using MongoDB.Driver;
using Shared.Configurations;

namespace Inventory.Gprc.Repositories
{
    public class InventoryRepository : MongoDbRepository<InventoryEntry>, IInventoryRepository
    {
        public InventoryRepository(IMongoClient client, MongoDbSettings setting) : base(client, setting)
        {
        }

        public async Task<int> GetStockQuantity(string itemNo) => Collection.AsQueryable()
            .Where(x => x.ItemNo.Equals(itemNo))
            .Sum(x => x.Quantity);
    }
}
