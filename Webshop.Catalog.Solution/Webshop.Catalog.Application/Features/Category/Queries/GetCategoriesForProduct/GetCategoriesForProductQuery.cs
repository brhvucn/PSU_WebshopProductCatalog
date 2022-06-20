using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Category.Application.Features.Category.Dtos;

namespace Webshop.Catalog.Application.Features.Category.Queries.GetCategoriesForProduct
{
    public class GetCategoriesForProductQuery : IQuery<IEnumerable<CategoryDto>>
    {
        public GetCategoriesForProductQuery(int productId)
        {
            ProductId = productId;
        }

        public int ProductId { get; private set; }
    }
}
