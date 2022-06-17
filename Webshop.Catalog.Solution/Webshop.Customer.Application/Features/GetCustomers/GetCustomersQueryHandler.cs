using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Customer.Application.Contracts.Persistence;
using Webshop.Customer.Application.Features.Dto;
using Webshop.Domain.Common;

namespace Webshop.Customer.Application.Features.GetCustomers
{
    public class GetCustomersQueryHandler : IQueryHandler<GetCustomersQuery, List<CustomerDto>>
    {
        private ILogger<GetCustomersQueryHandler> logger;
        private IMapper mapper;
        private ICustomerRepository customerRepository;
        public GetCustomersQueryHandler(ILogger<GetCustomersQueryHandler> logger, IMapper mapper, ICustomerRepository customerRepository)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.customerRepository = customerRepository;
        }

        public async Task<Result<List<CustomerDto>>> Handle(GetCustomersQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                var queryResult = await this.customerRepository.GetAll();
                List<CustomerDto> result = new List<CustomerDto>();
                foreach(var element in queryResult)
                {
                    result.Add(this.mapper.Map<CustomerDto>(element));
                }
                return Result.Ok<List<CustomerDto>>(result);
            }
            catch(Exception ex)
            {
                this.logger.LogCritical(ex, ex.Message);
                return Result.Fail<List<CustomerDto>>(Errors.General.UnspecifiedError(ex.Message));
            }
        }
    }
}
