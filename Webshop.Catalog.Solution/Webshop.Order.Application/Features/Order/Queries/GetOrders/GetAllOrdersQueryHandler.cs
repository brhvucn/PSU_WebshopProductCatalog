﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Domain.Common;
using Webshop.Order.Application.Contracts.Persistance;

namespace Webshop.Order.Application.Features.Order.Commands.CreateOrder
{
    internal class GetAllOrdersQueryHandler : ICommandHandler<CreateOrderCommand>
    {
        private ILogger<CreateOrderCommandHandler> logger;
        private IOrderRepository orderRepository;

        public GetAllOrdersQueryHandler(ILogger<CreateOrderCommandHandler> logger, IOrderRepository orderRepository)
        {
            this.logger = logger;
            this.orderRepository = orderRepository;
        }

        public Task<Result> Handle(CreateOrderCommand command, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
