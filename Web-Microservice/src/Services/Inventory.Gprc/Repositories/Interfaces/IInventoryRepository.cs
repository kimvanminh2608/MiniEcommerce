using Contracts.Domains.Interfaces;
using Inventory.Gprc.Entities;

namespace Inventory.Gprc.Repositories.Interfaces
{
    public interface IInventoryRepository : IMongoDbRepositoryBase<InventoryEntry>
    {
        Task<int> GetStockQuantity(string itemNo);
    }
}
