using AutoMapper;
using MediatR;
using Ordering.Application.Commons.Interfaces;
using Ordering.Application.Commons.Models;
using Ordering.Domain.Entities;
using Serilog;
using Shared.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.V1.Orders
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, ApiResult<OrderDto>>
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _repository;
        private readonly ILogger _logger;
        public UpdateOrderCommandHandler(IMapper mapper, IOrderRepository repository, ILogger logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        private const string MethodName = "UpdateOrderCommandHandler";
        public async Task<ApiResult<OrderDto>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var exist = await _repository.GetByIdAsync(request.Id);
            if (exist == null)
            {
                return null;
            }
            _logger.Information($"BEGIN {MethodName} - order: {request.Id}");
            var entity = _mapper.Map(request, exist);
            var updateOrder = await _repository.UpdateOrderAsync(entity);
            await _repository.SaveChangesAsync();
            _logger.Information($"Update order done");
            var result = _mapper.Map<OrderDto>(entity);
            _logger.Information($"END {MethodName} - order: {request.Id}");
            return new ApiSuccessResult<OrderDto>(result);
        }
    }
}
