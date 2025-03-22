using Infrastructure.Commons;
using Inventory.Grpc.Entities;
using Inventory.Grpc.Repositories.Interfaces;
using MongoDB.Driver;
using Shared.Configurations;

namespace Inventory.Grpc.Repositories
{
    public class InventoryRepository : MongoDbRepository<InventoryEntry>, IInventoryRepository
    {
        public InventoryRepository(IMongoClient client, MongoDbSettings setting) : base(client, setting)
        {
        }

        public async Task<int> GetStockQuantity(string itemNo) => Collection.AsQueryable()
            .Where(x => x.ItemNo == itemNo)
            .Sum(x => x.Quantity);
    }
}
