using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Webshop.Catalog.Application.Features.Product.Dtos;
using Webshop.Catalog.Application.Features.Product.Queries.GetProduct;

namespace Webshop.Catalog.Api.Controllers
{
    [Route("api/demoproduct")]
    [ApiController]
    public class DemoProductController : BaseController
    {
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            ProductDto productDto = new ProductDto
            {
                Id = id,
                Name = "Demo Product",
                SKU = "DEMO-SKU",
                Price = 10000,
                Currency = "DKK"
            };
            return FromResult<ProductDto>(productDto);
        }
    }
}
