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

namespace Webshop.Customer.Application.Features.UpdateCustomer
{
    public class UpdateCustomerCommandHandler : ICommandHandler<UpdateCustomerCommand>
    {
        private ILogger<UpdateCustomerCommandHandler> logger;
        private ICustomerRepository customerRepository;
        public UpdateCustomerCommandHandler(ILogger<UpdateCustomerCommandHandler> logger, ICustomerRepository customerRepository)
        {
            this.logger = logger;
            this.customerRepository = customerRepository;
        }

        public async Task<Result> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                await this.customerRepository.UpdateAsync(command.Customer);
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
