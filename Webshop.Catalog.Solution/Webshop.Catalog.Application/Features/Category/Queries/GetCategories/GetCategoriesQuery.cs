using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Catalog.Domain.AggregateRoots;
using Webshop.Category.Application.Features.Category.Dtos;

namespace Webshop.Category.Application.Features.Category.Queries.GetCategories
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
