using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Category.Application.Features.Category.Dtos;
using Webshop.Category.Application.Features.Category.Requests;

namespace Webshop.Catalog.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.AggregateRoots.Category, CategoryDto>().ReverseMap();            
            CreateMap<Domain.AggregateRoots.Category, CreateCategoryRequest>().ReverseMap();
            CreateMap<Domain.AggregateRoots.Category, UpdateCategoryRequest>().ReverseMap();
        }
    }
}
