using AutoMapper;
using Customer.API.Repositories.Interfaces;
using Customer.API.Services.Interfaces;
using Shared.DTOs.Customer;
using Shared.DTOs.Product;

namespace Customer.API.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;
        public CustomerService(ICustomerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper= mapper;
        }
        public async Task<IResult> GetCustomerByUserName(string userName) =>
            Results.Ok(await _repository.GetCustomerByUserNameAsync(userName));

        public async Task<IResult> GetCustomers() => 
            Results.Ok(await _repository.GetCustomersAsync());

        public async Task CreateCustomer(CustomerDto customerDto)
        {
            var customer = _mapper.Map<Entities.Customer>(customerDto);
            await _repository.CreateCustomer(customer);
            await _repository.SaveChangesAsync();
            var result = _mapper.Map<CustomerDto>(customer);
            Results.Ok(result);
        }
        
        public async Task UpdateCustomer(int id, CustomerDto customerDto)
        {
            var customer = _repository.GetByIdAsync(id);
            if (customer == null)
            {
                Results.Ok(null);
            }
            var updateCustomer = _mapper.Map<Entities.Customer>(customerDto);
            await _repository.UpdateCustomer(updateCustomer);
            await _repository.SaveChangesAsync();
            var result = _mapper.Map<CustomerDto>(customer);
            Results.Ok(result);
        }

        public async Task DeleteCustomer (int id)
        {
            var customer = _repository.GetByIdAsync(id);
            if (customer == null)
            {
                Results.Ok(null);
            }

            await _repository.DeleteCustomer(id);
            await _repository.SaveChangesAsync();
            Results.Ok("Deleted Customer");
        }
    }
}
