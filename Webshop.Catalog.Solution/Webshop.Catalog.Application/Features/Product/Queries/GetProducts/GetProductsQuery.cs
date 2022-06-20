using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Catalog.Application.Features.Product.Dtos;

namespace Webshop.Catalog.Application.Features.Product.Queries.GetProducts
{
    public class GetProductsQuery : IQuery<IEnumerable<ProductDto>>
    {
        public GetProductsQuery(int categoryId)
        {
            CategoryId = categoryId;
        }

        public int CategoryId { get; private set; }
    }
}
