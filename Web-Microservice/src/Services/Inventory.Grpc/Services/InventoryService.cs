using Grpc.Core;
using Inventory.Grpc.Protos;
using Inventory.Grpc.Repositories.Interfaces;
using System;
using Ilogger = Serilog.ILogger;
namespace Inventory.Grpc.Services
{
    public class InventoryService : StockProtoService.StockProtoServiceBase
    {
        private readonly IInventoryRepository _repository;
        private readonly Ilogger _logger;
        public InventoryService(IInventoryRepository repository, Ilogger logger)
        {
            _repository = repository ?? throw new ArgumentException(nameof(repository));
            _logger = logger ?? throw new ArgumentException(nameof(logger));
        }

        override public async Task<StockModel> GetStock(GetStockRequest request, ServerCallContext context)
        {
            _logger.Information($"Begin get stock of itemno {request.ItemNo}");
            var stockQuantity = await _repository.GetStockQuantity(request.ItemNo);
            var result = new StockModel { Quantity = stockQuantity };
            _logger.Information($"End get stock of itemno {request.ItemNo} - quantity: {result.Quantity}");
            return result;
        }
    }
}
