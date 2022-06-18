using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Catalog.Application.Features.Catalog.Dtos;

namespace Webshop.Catalog.Application.Features.Catalog.Queries.GetChildCategories
{
    public class GetChildCategoriesQuery : IQuery<List<CategoryDto>>
    {
        public GetChildCategoriesQuery(int parentCategoryId)
        {
            ParentCategoryId = parentCategoryId;
        }

        public int ParentCategoryId { get; private set; }
    }
}
