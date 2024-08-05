using Contracts.Commons.Interfaces;
using Infrastructure.Commons;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Commons.Interfaces;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBaseAsync<Order, long, OrderContext>, IOrderRepository
    {
        public OrderRepository(OrderContext dbContext, IUnitOfWork<OrderContext> unitOfWork) :base(dbContext, unitOfWork)
        {
            
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            await CreateAsync(order);
            return order;
        }

        public async Task<IEnumerable<Order>> GetOrderByUsername(string userName)
        {
            var result = await FindCondition(x => x.UserName.Equals(userName)).ToListAsync();
            return result;
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            await UpdateAsync(order);
            return order;
        }
    }
}
