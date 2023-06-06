using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Domain.Common;
using Webshop.Order.Application.Contracts.Persistence;

namespace Webshop.Order.Application.Features.Order.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand>
    {
        private ILogger<CreateOrderCommandHandler> logger;
        private IOrderRepository orderRepository;

        public CreateOrderCommandHandler(ILogger<CreateOrderCommandHandler> logger, IOrderRepository orderRepository)
        {
            this.logger = logger;
            this.orderRepository = orderRepository;
        }

        public async Task<Result> Handle(CreateOrderCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                Domain.AggregateRoots.Order newOrder = new Domain.AggregateRoots.Order(command.Customer, command.DateOfIssue, command.DueDate, command.Discount, command.OrderProducts);
                await this.orderRepository.CreateAsync(newOrder);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                this.logger.LogCritical(ex, ex.Message);
                return Result.Fail(Errors.General.UnspecifiedError(ex.Message));
            }
        }
    }
}
