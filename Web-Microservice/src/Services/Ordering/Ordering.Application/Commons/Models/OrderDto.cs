using Ordering.Application.Commons.Mappings;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Commons.Models
{
    public class OrderDto : IMapFrom<Order>
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }

        public string ShippingAddress { get; set; }
        public string InvoiceAddress { get; set; }
        public string Status { get; set; }
    }
}
