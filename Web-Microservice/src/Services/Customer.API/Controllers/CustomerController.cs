using Customer.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Customer;

namespace Customer.API.Controllers
{
    public static class CustomerController
    {
        public static void MapCustomersAPI(this WebApplication app)
        {
            //Minimal API
            app.MapGet("/api/customers", async (ICustomerService customerService) => await customerService.GetCustomers());
            app.MapGet("/api/customers/{username}", async (string username, ICustomerService customerService) =>
                        await customerService.GetCustomerByUserName(username));

            app.MapPost("/api/customers/create-customer", async (CustomerDto customerDto, ICustomerService customerService) =>
            {
                await customerService.CreateCustomer(customerDto);
            });

            app.MapPut("/api/customers/update-customer/{id}", async (int id, CustomerDto customerDto, ICustomerService customerService) =>
            {
                await customerService.UpdateCustomer(id, customerDto);
            });

            app.MapDelete("/api/customers/delete-customer/{id}", async (int id, ICustomerService customerService) =>
            {
                await customerService.DeleteCustomer(id);
            });
        }
    }
}
