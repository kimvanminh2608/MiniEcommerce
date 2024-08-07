using MediatR;
using Ordering.Application.Commons.Interfaces;
using Ordering.Application.Commons.Models;
using Serilog;
using Shared.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.V1.Orders
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, ApiResult<long>>
    {
        private readonly IOrderRepository _repository;
        private readonly ILogger _logger;
        public DeleteOrderCommandHandler(IOrderRepository repository, ILogger logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ApiResult<long>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);
            _repository.Delete(entity);
            entity.DeletedOrder();
            await _repository.SaveChangesAsync();
            _logger.Information($"Order {request.Id} deleted");
            return new ApiSuccessResult<long>(204);
        }
    }
}
