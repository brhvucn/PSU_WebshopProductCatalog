using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Catalog.Application.Features.Catalog.Dtos;
using Webshop.Catalog.Domain.AggregateRoots;

namespace Webshop.Catalog.Application.Features.Catalog.Queries.GetCategories
{
    public class GetCategoriesQuery : IQuery<List<CategoryDto>>
    {
        public GetCategoriesQuery(bool includeChildCategories)
        {
            IncludeChildCategories = includeChildCategories;
        }

        public bool IncludeChildCategories { get; private set; }
    }
}
