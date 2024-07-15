using Contracts.Commons.Interfaces;
using Customer.API.Persistence;
using Customer.API.Repositories.Interfaces;
using Infrastructure.Commons;
using Microsoft.EntityFrameworkCore;

namespace Customer.API.Repositories
{
    public class CustomerRepository : RepositoryBaseAsync<Entities.Customer, int, CustomerContext>, ICustomerRepository
    {
        public CustomerRepository(CustomerContext dbContext, IUnitOfWork<CustomerContext> unitOfWork) : base(dbContext, unitOfWork)
        {
            
        }

        public async Task<Entities.Customer> GetCustomerByUserNameAsync(string userName) =>
            await FindCondition(x => x.UserName.Equals(userName)).SingleOrDefaultAsync();

        public async Task<IEnumerable<Entities.Customer>> GetCustomersAsync() =>
            await FindAll().ToListAsync();

        public Task CreateCustomer(Entities.Customer customer) => CreateAsync(customer);
        public Task UpdateCustomer(Entities.Customer customer) => UpdateAsync(customer);
        public async Task DeleteCustomer(int customerId)
        {
            var customer = await GetCustomerById(customerId);
            if (customer != null) DeleteAsync(customer);
        }
        private async Task<Entities.Customer> GetCustomerById(int customerId) =>
            await FindCondition(x => x.Id == customerId).SingleOrDefaultAsync();
        
    }
}
