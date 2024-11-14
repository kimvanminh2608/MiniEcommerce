using AutoMapper;
using Infrastructure.Commons;
using Infrastructure.Commons.Models;
using Infrastructure.Extensions;
using Inventory.Product.API.Entities;
using Inventory.Product.API.Extensions;
using Inventory.Product.API.Services.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using Shared.Configurations;
using Shared.DTOs.Inventory;

namespace Inventory.Product.API.Services
{
    public class InventoryService : MongoDbRepository<InventoryEntry>, IInventoryService
    {
        private readonly IMapper _mapper;
        public InventoryService(IMongoClient client, MongoDbSettings setting, IMapper mapper) : base(client, setting)
        {
            _mapper = mapper;
        }
        public async Task<IEnumerable<InventoryEntryDto>> GetAllByItemNoAsync(string itemNo)
        {
            var entities = await FindAll().Find(x => x.ItenNo.Equals(itemNo)).ToListAsync();
            var result = _mapper.Map<IEnumerable<InventoryEntryDto>>(entities);
            return result;
        }

        public async Task<PagedList<InventoryEntryDto>> GetAllByItemNoPagingAsync(GetInventoryPagingQuery query)
        {
            var filterSearchTern = Builders<InventoryEntry>.Filter.Empty;
            var filterItemNo = Builders<InventoryEntry>.Filter.Eq(x => x.ItenNo, query.ItemNo());
            if(!string.IsNullOrEmpty(query.SearchTerm))
            {
                filterSearchTern = Builders<InventoryEntry>.Filter.Eq(x => x.DocumentNo, query.SearchTerm);
            }
            var andFilter = filterItemNo & filterSearchTern;
            var pagedList = await Collection.PaginatedListAsync(andFilter,
                pageIndex: query.PageNumber,
                pageSize: query.PageSize);

            var items = _mapper.Map<IEnumerable<InventoryEntryDto>>(pagedList);

            var result = new PagedList<InventoryEntryDto>(items, pagedList.GetMetaData().TotalItems, query.PageNumber, query.PageSize);
            return result;
        }

        public async Task<InventoryEntryDto> GetByIdAsync(string id)
        {
            FilterDefinition<InventoryEntry> filter = Builders<InventoryEntry>.Filter.Eq(x => x.Id, id);
            var entity = await FindAll().Find(filter).FirstOrDefaultAsync();
            var result = _mapper.Map<InventoryEntryDto>(entity);

            return result;
        }

        public async Task<InventoryEntryDto> PurchaseItemAsync(string itemNo, PurchaseProductDto model)
        {
            var itemToAdd = new InventoryEntry(ObjectId.GenerateNewId().ToString())
            {
                ItenNo = itemNo,
                Quantity = model.Quantity,
                DocumentType = model.DocumentType,
            };

            await CreateAsync(itemToAdd);

            var result = _mapper.Map<InventoryEntryDto>(itemToAdd);
            return result;
        }
    }
}
