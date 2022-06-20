using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Catalog.Application.Features.Product.Dtos;
using Webshop.Catalog.Application.Features.Product.Queries.GetProducts;
using Webshop.Category.Application.Features.Category.Commands.CreateCategory;
using Webshop.Category.Application.Features.Category.Commands.DeleteCategory;
using Webshop.Category.Application.Features.Category.Commands.UpdateCategory;
using Webshop.Category.Application.Features.Category.Dtos;
using Webshop.Category.Application.Features.Category.Queries.GetCategories;
using Webshop.Category.Application.Features.Category.Queries.GetCategory;
using Webshop.Category.Application.Features.Category.Queries.GetChildCategories;
using Webshop.Category.Application.Features.Category.Requests;
using Webshop.Domain.Common;

namespace Webshop.Catalog.Api.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : BaseController
    {
        private IDispatcher dispatcher;
        private ILogger<CategoryController> logger;
        private IMapper mapper;
        public CategoryController(IDispatcher dispatcher, IMapper mapper, ILogger<CategoryController> logger)
        {
            this.dispatcher = dispatcher;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody]CreateCategoryRequest request)
        {
            //check incoming request
            CreateCategoryRequest.Validator validator = new CreateCategoryRequest.Validator();
            var result = await validator.ValidateAsync(request);
            if (result.IsValid)
            {
                CreateCategoryCommand command = new CreateCategoryCommand(request.Name, request.Description, request.ParentId);
                Result commandResult = await dispatcher.Dispatch(command);
                if (commandResult.Success)
                {
                    return Ok();
                }
                else
                {
                    return Error(commandResult.Error);
                }
            }
            else
            {
                this.logger.LogError(string.Join(",", result.Errors.Select(x => x.ErrorMessage)));
                return Error(result.Errors);
            }            
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateCategory([FromBody]UpdateCategoryRequest request, int id)
        {
            //check incoming request
            UpdateCategoryRequest.Validator validator = new UpdateCategoryRequest.Validator();
            var result = await validator.ValidateAsync(request);
            if (result.IsValid)
            {
                UpdateCategoryCommand command = new UpdateCategoryCommand(request.Name, request.Description, id);
                await dispatcher.Dispatch(command);
                return Ok();
            }
            else
            {
                this.logger.LogError(string.Join(",", result.Errors.Select(x => x.ErrorMessage)));
                return Error(result.Errors);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            DeleteCategoryCommand command = new DeleteCategoryCommand(id);
            await dispatcher.Dispatch(command);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetRootCategories(bool includeChildren)
        {
            GetCategoriesQuery query = new GetCategoriesQuery(includeChildren);
            var result = await this.dispatcher.Dispatch(query);
            if (result.Success)
            {
                return FromResult<List<CategoryDto>>(result);
            }
            else
            {
                this.logger.LogError(string.Join(",", result.Error.Message));
                return Error(result.Error);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            GetCategoryQuery query = new GetCategoryQuery(id);
            var result = await this.dispatcher.Dispatch(query);
            if (result.Success)
            {
                return FromResult<CategoryDto>(result);
            }
            else
            {
                this.logger.LogError(string.Join(",", result.Error.Message));
                return Error(result.Error);
            }
        }

        [HttpGet]
        [Route("{id}/categories")]
        public async Task<IActionResult> GetChildCategories(int id)
        {
            GetChildCategoriesQuery query = new GetChildCategoriesQuery(id);
            var result = await this.dispatcher.Dispatch(query);
            if (result.Success)
            {
                return FromResult<IEnumerable<CategoryDto>>(result);
            }
            else
            {
                this.logger.LogError(string.Join(",", result.Error.Message));
                return Error(result.Error);
            }
        }

        [HttpGet]
        [Route("{id}/products")]
        public async Task<IActionResult> GetCategoryProducts(int id)
        {
            GetProductsQuery getProductsQuery = new GetProductsQuery(id);
            var queryResult = await this.dispatcher.Dispatch(getProductsQuery);
            return FromResult<IEnumerable<ProductDto>>(queryResult);
        }
    }
}
