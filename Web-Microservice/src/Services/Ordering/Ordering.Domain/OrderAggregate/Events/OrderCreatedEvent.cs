using Contracts.Commons.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.OrderAggregate.Events
{
    public class OrderCreatedEvent : BaseEvent
    {
        public long Id { get; private set; }
        public string UserName { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string EmailAddress { get; private set; }
        public decimal TotalPrice { get; private set; }
        public string ShippingAddress { get; private set; }
        public string InvoiceAddress { get; private set; }

        public OrderCreatedEvent(
            long id,
            string userName,
            string firstName,
            string lastName,
            string emailAddress,
            decimal totalPrice,
            string shippingAddress,
            string invoiceAddress)
        {
            Id = id;
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
            TotalPrice = totalPrice;
            ShippingAddress = shippingAddress;
            InvoiceAddress = invoiceAddress;
        }
    }
}
