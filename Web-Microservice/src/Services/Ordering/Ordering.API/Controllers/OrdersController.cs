using Contracts.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Commons.Models;
using Ordering.Application.Features.V1.Orders;
using Shared.SeedWork;
using Shared.Services.Email;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Ordering.API.Controllers
{
    [Route("api/v1/[Controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ISMTPEmailService _smptEmailService;

        public OrdersController(IMediator mediator, ISMTPEmailService smptEmailService)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(_mediator));
            _smptEmailService = smptEmailService;
        }
        private static class RouteNames
        {
            public const string GetOrders = nameof(GetOrders);
            public const string CreateOrders = nameof(CreateOrders);
            public const string DeleteOrders = nameof(DeleteOrders);
            public const string UpdateOrders = nameof(UpdateOrders);
        }

        [HttpGet("{username}", Name = RouteNames.GetOrders)]
        [ProducesResponseType(typeof(IEnumerable<OrderDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByUsername([Required] string username)
        {
            var query = new GetOrdersQuery(username);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost(Name = RouteNames.CreateOrders)]
        [ProducesResponseType(typeof(ApiResult<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApiResult<long>>> CreateOrder([FromBody] CreateOrderCommand request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPut("{id:long}", Name = RouteNames.UpdateOrders)]
        [ProducesResponseType(typeof(ApiResult<OrderDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApiResult<OrderDto>>> UpdateOrder([Required] long id, [FromBody] UpdateOrderCommand request)
        {
            request.SetId(id);
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpDelete("{id:long}",Name = RouteNames.DeleteOrders)]
        [ProducesResponseType(typeof(ApiResult<long>), (int)HttpStatusCode.NoContent)]
        public async Task<ActionResult<ApiResult<long>>> DeleteOrder([Required] long id)
        {
            var command = new DeleteOrderCommand(id);
            var result = await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("test-email")]
        public async Task<IActionResult> TestEmail()
        {
            var message = new MailRequest
            {
                Body = "Hello",
                Subject = "Test",
                ToAddress = "kimvanminh2608@gmail.com"
            };

            await _smptEmailService.SendEmailAsync(message);
            return Ok();
        }
    }
}
