using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Webshop.Application.Contracts;
using Webshop.Domain.Common;
using Webshop.Order.Application.Features.Order.Dtos;
using Webshop.Order.Application.Features.Order.Queries.GetOrderByCustomer;
using Webshop.Order.Application.Features.Order.Commands.CreateOrder;
using Webshop.Order.Application.Features.Order.Requests;
using Webshop.Order.Application.Features.Order.Commands.DeleteOrder;
using Webshop.Order.Application.Features.Order.Queries.GetOrders;

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

        /// <summary>
        /// Creates a new order with the specified customer, date of issue, due date, and ordered items.
        /// </summary>
        /// <param name="request">The request containing order details.</param>
        /// <returns>The result of the order creation operation.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            CreateOrderRequest.Validator validator = new CreateOrderRequest.Validator();
            var result = await validator.ValidateAsync(request);
            if (result.IsValid)
            {
                CreateOrderCommand command = new CreateOrderCommand(request.Customer, request.DateOfIssue, request.DueDate, request.Discount, request.OrderedProducts);
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

        /// <summary>
        /// Retrieves the Orders with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the order to retrieve</param>
        /// <returns>The order corresponding to the specified ID, if found</returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            GetOrdersByCustomerQuery query = new GetOrdersByCustomerQuery(id);
            var result = await dispatcher.Dispatch(query);
            if (result.Success)
            {
                return FromResult<IEnumerable<OrderDto>>(result);
            }
            else
            {
                this.logger.LogError(string.Join(",", result.Error.Message));
                return Error(result.Error);
            }
        }

        /// <summary>
        /// Returns a collection of all order objects from the database
        /// </summary>
        /// <returns>Collection of OrderDto </returns>
        [Route("/getall")]
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            GetAllOrdersQuery query = new GetAllOrdersQuery();
            var result = await dispatcher.Dispatch(query);
            if (result.Success)
            {
                return FromResult<IEnumerable<OrderDto>>(result);
            }
            else
            {
                this.logger.LogError(string.Join(",", result.Error.Message));
                return Error(result.Error);
            }
        }

        //Get orders by Customers
        /*[HttpGet]
        [Route("{customerId}")]
        public async Task<IActionResult> GetOrderByCustomer(int customerId)
        {
            {
                GetOrdersByCustomer query = new GetOrdersByCustomer(customerId);
                var result = await this.dispatcher.Dispatch(query);
                return FromResult<IEnumerable<OrderDto>>(result);
            }
        }*/

        /// <summary>
        /// Deletes the order by the specified ID
        /// </summary>
        /// <param name="orderId">The ID of the order to delete.</param>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            GetOrderCommand command = new GetOrderCommand(id);
            var commandResult = await this.dispatcher.Dispatch(command);
            return FromResult(commandResult);
        }
    }
}

