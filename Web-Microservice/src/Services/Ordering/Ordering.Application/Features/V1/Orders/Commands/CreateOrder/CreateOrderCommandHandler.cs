using AutoMapper;
using Contracts.Services;
using MediatR;
using Ordering.Application.Commons.Interfaces;
using Ordering.Application.Commons.Models;
using Ordering.Domain.Entities;
using Serilog;
using Shared.SeedWork;
using Shared.Services.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.V1.Orders
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ApiResult<long>>
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _repository;
        private readonly ILogger _logger;
        private readonly ISMTPEmailService _emailService;
        public CreateOrderCommandHandler(IMapper mapper, IOrderRepository repository, ILogger logger, ISMTPEmailService emailService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }
        private const string MethodName = "CreateOrderCommandHandler";
        public async Task<ApiResult<long>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.Information($"BEGIN: {MethodName} - UserName: {request.UserName}");
            var order = _mapper.Map<Order>(request);
            var result = await _repository.CreateOrderAsync(order);
            await _repository.SaveChangesAsync();
            _logger.Information($"CREATE Order UserName: {request.UserName} success");
            SendMailAsync(result, cancellationToken);
            _logger.Information($"END: {MethodName} - UserName: {request.UserName}");
            return new ApiSuccessResult<long>(result.Id);
        }

        private async Task SendMailAsync(Order order, CancellationToken cancellationToken)
        {
            var emailRequest = new MailRequest
            {
                ToAddress = order.EmailAddress,
                Body = "Order create succesfully",
                Subject = "Order create succesfully"
            };

            try
            {
                await _emailService.SendEmailAsync(emailRequest, cancellationToken);
                _logger.Information($"Sent Created order to email {order.EmailAddress}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Order {order.Id} error: {ex}");
            }
        }
    }
}
