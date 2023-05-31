using Webshop.Catalog.Application.Features.Product.Commands.CreateProduct;

namespace Webshop.Catalog.Application.Test
{
    public class ProductTests
    {
        [Fact]
        public void CreateProductCommand_InValid_ExpectFailure()
        {
            Action action = () => new CreateProductCommand(null, null, 0, null);
            Assert.Throws<ArgumentNullException>(action);
        }
    }
}