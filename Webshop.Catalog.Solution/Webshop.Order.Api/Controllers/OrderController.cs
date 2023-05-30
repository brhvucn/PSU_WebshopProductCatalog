using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Webshop.Application.Contracts;
using Webshop.Order.Application.Features.Category.Requests;
using Webshop.Order.Application.Features.Category.Commands.CreateOrder;
using Webshop.Domain.Common;

namespace Webshop.Order.Api.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : BaseController
    {
        private IDispatcher dispatcher;
        private ILogger<OrderController> logger;
        private IMapper mapper;

        public OrderController(IDispatcher dispatcher, IMapper mapper, ILogger<OrderController> logger) 
        {
            this.dispatcher = dispatcher;
            this.mapper = mapper;
            this.logger = logger;  
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        { 
            CreateOrderRequest.Validator validator = new CreateOrderRequest.Validator();
            var result = await validator.ValidateAsync(request);
            if (result.IsValid)
            {
                CreateOrderCommand command = new CreateOrderCommand(request.Customer, request.DateOfIssue, request.DueDate, request.OrderedProducts);
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
    }
}
