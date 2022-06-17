using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Customer.Application.Features.CreateCustomer;
using Webshop.Customer.Application.Features.DeleteCustomer;
using Webshop.Customer.Application.Features.GetCustomer;
using Webshop.Customer.Application.Features.Requests;
using Webshop.Customer.Application.Features.UpdateCustomer;
using Webshop.Domain.Common;

namespace Webshop.Customer.Api.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomersController : BaseController
    {
        private IDispatcher dispatcher;
        private IMapper mapper;
        public CustomersController(IDispatcher dispatcher, IMapper mapper)
        {
            this.mapper = mapper;
            this.dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            return BadRequest(new { description = "This endpoint has not been implemented" });
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            GetCustomerQuery query = new GetCustomerQuery(id);
            Result result = await this.dispatcher.Dispatch(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody]CreateCustomerRequest request)
        {
            CreateCustomerRequest.Validator validator = new CreateCustomerRequest.Validator();
            var result = validator.Validate(request);
            if (result.IsValid)
            {
                Domain.AggregateRoots.Customer customer = this.mapper.Map<Domain.AggregateRoots.Customer>(request);
                CreateCustomerCommand command = new CreateCustomerCommand(customer);
                Result createResult = await this.dispatcher.Dispatch(command);
                return Ok(createResult);
            }
            else
            {
                return Error(result.Errors);
            }            
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            DeleteCustomerCommand command = new DeleteCustomerCommand(id);
            Result result = await this.dispatcher.Dispatch(command);
            return Ok(result);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateCustomer([FromBody]UpdateCustomerRequest request)
        {
            UpdateCustomerRequest.Validator validator = new UpdateCustomerRequest.Validator();
            var result = validator.Validate(request);
            if (result.IsValid)
            {
                Domain.AggregateRoots.Customer customer = this.mapper.Map<Domain.AggregateRoots.Customer>(request);
                UpdateCustomerCommand command = new UpdateCustomerCommand(customer);
                Result createResult = await this.dispatcher.Dispatch(command);
                return Ok(createResult);
            }
            else
            {
                return Error(result.Errors);
            }
        }
    }
}
