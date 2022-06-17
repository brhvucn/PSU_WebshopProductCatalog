using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Customer.Application.Contracts.Persistence;
using Webshop.Domain.Common;

namespace Webshop.Customer.Application.Features.CreateCustomer
{
    public class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand>
    {
        private ILogger<CreateCustomerCommandHandler> logger;
        private ICustomerRepository customerRepository;
        public CreateCustomerCommandHandler(ILogger<CreateCustomerCommandHandler> logger, ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
            this.logger = logger;
        }

        public async Task<Result> Handle(CreateCustomerCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                await this.customerRepository.CreateAsync(command.Customer);
                return Result.Ok();
            }
            catch(Exception ex)
            {
                this.logger.LogCritical(ex, ex.Message);
                return Result.Fail(Errors.General.UnspecifiedError(ex.Message));
            }
        }
    }
}
