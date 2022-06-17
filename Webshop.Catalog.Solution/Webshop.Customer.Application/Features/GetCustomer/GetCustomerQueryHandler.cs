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

namespace Webshop.Customer.Application.Features.GetCustomer
{
    public class GetCustomerQueryHandler : IQueryHandler<GetCustomerQuery, CustomerDto>
    {
        private ILogger<GetCustomerQueryHandler> logger;
        private IMapper mapper;
        private ICustomerRepository customerRepository;
        public GetCustomerQueryHandler(ILogger<GetCustomerQueryHandler> logger, IMapper mapper, ICustomerRepository customerRepository)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.customerRepository = customerRepository;
        }

        public async Task<Result<CustomerDto>> Handle(GetCustomerQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                Domain.AggregateRoots.Customer customer = await this.customerRepository.GetById(query.CustomerId);
                return this.mapper.Map<CustomerDto>(customer);
            }
            catch(Exception ex)
            {
                this.logger.LogCritical(ex, ex.Message);
                return Result.Fail<CustomerDto>(Errors.General.UnspecifiedError(ex.Message));
            }
        }
    }
}
