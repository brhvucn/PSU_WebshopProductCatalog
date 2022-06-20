using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Catalog.Application.Features.Product.Dtos;

namespace Webshop.Catalog.Application.Features.Product.Queries.GetProduct
{
    public class GetProductQuery : IQuery<ProductDto>
    {
        public GetProductQuery(int productId)
        {
            ProductId = productId;
        }

        public int ProductId { get; private set; }
    }
}
