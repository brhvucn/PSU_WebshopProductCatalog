using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Category.Application.Features.Category.Dtos;

namespace Webshop.Category.Application.Features.Category.Queries.GetChildCategories
{
    public class GetChildCategoriesQuery : IQuery<IEnumerable<CategoryDto>>
    {
        public GetChildCategoriesQuery(int parentCategoryId)
        {
            ParentCategoryId = parentCategoryId;
        }

        public int ParentCategoryId { get; private set; }
    }
}
