using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Catalog.Application.Features.Product.Commands.AttachProductToCategory;
using Webshop.Catalog.Application.Features.Product.Commands.DetachProductFromCategory;
using Webshop.Catalog.Application.Features.Product.Commands.CreateProduct;
using Webshop.Catalog.Application.Features.Product.Commands.DeleteProduct;
using Webshop.Catalog.Application.Features.Product.Commands.UpdateProduct;
using Webshop.Catalog.Application.Features.Product.Dtos;
using Webshop.Catalog.Application.Features.Product.Queries.GetProduct;
using Webshop.Catalog.Application.Features.Product.Queries.GetProducts;
using Webshop.Catalog.Application.Features.Product.Requests;
using Webshop.Catalog.Application.Features.Category.Queries.GetCategoriesForProduct;
using Webshop.Category.Application.Features.Category.Dtos;

namespace Webshop.Catalog.Api.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : BaseController
    {
        private IDispatcher dispatcher;
        private ILogger<ProductController> logger;
        private IMapper mapper;

        public ProductController(IDispatcher dispatcher, ILogger<ProductController> logger, IMapper mapper)
        {
            this.dispatcher = dispatcher;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody]CreateProductRequest request)
        {
            CreateProductRequest.Validator validator = new CreateProductRequest.Validator();
            var validationResult = await validator.ValidateAsync(request);
            if (validationResult.IsValid)
            {
                CreateProductCommand command = new CreateProductCommand(request.Name, request.SKU, request.Price, request.Currency);
                var commandResult = await this.dispatcher.Dispatch(command);
                return FromResult(commandResult);
            }
            else
            {
                this.logger.LogError(string.Join(",", validationResult.Errors.Select(e=>e.ErrorMessage)));
                return Error(validationResult.Errors);
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductRequest request, int id)
        {
            UpdateProductRequest.Validator validator = new UpdateProductRequest.Validator();
            var validationResult = await validator.ValidateAsync(request);
            if (validationResult.IsValid)
            {
                UpdateProductCommand command = new UpdateProductCommand(request.Id, request.Name, request.Description, request.SKU, request.AmountInStock, request.Price, request.Currency, request.MinStock);
                var commandResult = await this.dispatcher.Dispatch(command);
                return FromResult(commandResult);
            }
            else
            {
                this.logger.LogError(string.Join(",", validationResult.Errors.Select(e => e.ErrorMessage)));
                return Error(validationResult.Errors);
            }
        }

        [HttpPut]
        [Route("{productid}/categories/{categoryid}")]
        public async Task<IActionResult> AttachProductToCategory(int productid, int categoryid)
        {
           AttachProductToCategoryCommand command = new AttachProductToCategoryCommand(productid, categoryid);
            var result = await this.dispatcher.Dispatch(command);
            return Ok();
        }

        [HttpDelete]
        [Route("{productid}/categories/{categoryid}")]
        public async Task<IActionResult> DetachProductFromCategory(int productid, int categoryid)
        {
            DetachProductFromCategoryCommand command = new DetachProductFromCategoryCommand(productid, categoryid);
            var result = await this.dispatcher.Dispatch(command);
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            DeleteProductCommand command = new DeleteProductCommand(id);
            var commandResult = await this.dispatcher.Dispatch(command);
            return FromResult(commandResult);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            GetProductQuery query = new GetProductQuery(id);
            var result = await this.dispatcher.Dispatch(query);
            return FromResult<ProductDto>(result);
        }

        [HttpGet]
        [Route("{id}/categories")]
        public async Task<IActionResult> GetProductCategories(int id)
        {
            GetCategoriesForProductQuery query = new GetCategoriesForProductQuery(id);
            var result = await this.dispatcher.Dispatch(query);
            return FromResult<IEnumerable<CategoryDto>>(result);
        }
    }
}
