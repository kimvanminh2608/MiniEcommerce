using AutoMapper;
using Shared.DTOs.Customer;
using Infrastructure.Mappings;

namespace Customer.API
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<Entities.Customer, CustomerDto>().IgnoreAllNonExisting();
            
        }
    }
}
