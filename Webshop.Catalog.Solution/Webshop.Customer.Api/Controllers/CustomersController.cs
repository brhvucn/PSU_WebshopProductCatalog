using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Customer.Application.Features.CreateCustomer;
using Webshop.Customer.Application.Features.DeleteCustomer;
using Webshop.Customer.Application.Features.Dto;
using Webshop.Customer.Application.Features.GetCustomer;
using Webshop.Customer.Application.Features.GetCustomers;
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
        private ILogger<CustomersController> logger;
        public CustomersController(IDispatcher dispatcher, IMapper mapper, ILogger<CustomersController> logger)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            GetCustomersQuery query = new GetCustomersQuery();
            Result<List<CustomerDto>> result = await this.dispatcher.Dispatch(query);
            if (result.Success)
            {
                return FromResult<List<CustomerDto>>(result);
            }
            else
            {
                this.logger.LogError(result.Error.Message);
                return Error(result.Error);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            GetCustomerQuery query = new GetCustomerQuery(id);
            Result<CustomerDto> result = await this.dispatcher.Dispatch(query);
            if(result.Success)
            {
                return FromResult<CustomerDto>(result);
            }
            else
            {
                this.logger.LogError(result.Error.Message);
                return Error(result.Error);
            }
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
                this.logger.LogError(string.Join(",", result.Errors.Select(x => x.ErrorMessage)));
                return Error(result.Errors);
            }            
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            DeleteCustomerCommand command = new DeleteCustomerCommand(id);
            Result result = await this.dispatcher.Dispatch(command);
            if (result.Success)
            {
                return FromResult(result);
            }
            else
            {
                this.logger.LogError(string.Join(",", result.Error.Message));
                return Error(result.Error);
            }
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
                this.logger.LogError(string.Join(",", result.Errors.Select(x => x.ErrorMessage)));
                return Error(result.Errors);
            }
        }
    }
}
