using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Category.Application.Features.Category.Dtos;

namespace Webshop.Category.Application.Features.Category.Queries.GetCategory
{
    public class GetCategoryQuery : IQueryHandler<CategoryDto>
    {
        public GetCategoryQuery(int categoryId)
        {
            CategoryId = categoryId;
        }

        public int CategoryId { get; private set; }
    }
}
