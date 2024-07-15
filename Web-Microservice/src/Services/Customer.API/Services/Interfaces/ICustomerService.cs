using Shared.DTOs.Customer;

namespace Customer.API.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<IResult> GetCustomerByUserName(string userName);
        Task<IResult> GetCustomers();
        Task CreateCustomer(CustomerDto customerDto);
        Task UpdateCustomer(int id, CustomerDto customerDto);
        Task DeleteCustomer(int id);
    }
}
