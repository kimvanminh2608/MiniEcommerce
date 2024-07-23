using MediatR;
using Ordering.Application.Commons.Mappings;
using Ordering.Application.Commons.Models;
using Ordering.Domain.Entities;
using Shared.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.V1.Orders
{
    public class UpdateOrderCommand : IRequest<ApiResult<OrderDto>>, IMapFrom<Order>
    {
        public long Id { get; private set; }

        public void SetId(long id)
        {
            Id = id;
        }

        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string InvoiceAddress { get; set; }
        public string ShippingAddress { get; set; }
    }
}
