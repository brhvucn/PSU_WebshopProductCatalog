using AutoMapper;
using Webshop.Customer.Application.Features.Dto;

namespace Webshop.Customer.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.AggregateRoots.Customer, CustomerDto>().ReverseMap();            
        }
    }
}
