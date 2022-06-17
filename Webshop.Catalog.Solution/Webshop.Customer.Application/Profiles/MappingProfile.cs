using AutoMapper;
using Webshop.Customer.Application.Features.Dto;
using Webshop.Customer.Application.Features.Requests;

namespace Webshop.Customer.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.AggregateRoots.Customer, CustomerDto>().ReverseMap();     
            CreateMap<Domain.AggregateRoots.Customer, CreateCustomerRequest>().ReverseMap();
            CreateMap<Domain.AggregateRoots.Customer, UpdateCustomerRequest>().ReverseMap();
        }
    }
}
