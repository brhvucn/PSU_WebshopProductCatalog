using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Domain.Common;
using Webshop.Order.Application.Contracts.Persistence;
using Webshop.Order.Application.Features.Order.Dtos;
using Webshop.Order.Application.Features.Order.Queries.GetOrderByCustomer;
using Webshop.Order.Application.Features.Order.Queries.GetOrders;

namespace Webshop.Order.Application.Features.Order.Queries.GetOrdersByCustomer
{
    public class GetOrdersByCustomerQueryHandler : IQueryHandler<GetOrdersByCustomerQuery, IEnumerable<OrderDto>>
    {
        private ILogger<GetOrdersByCustomerQueryHandler> logger;
        private IMapper mapper;
        private IOrderRepository orderRepository;

        public GetOrdersByCustomerQueryHandler(ILogger<GetOrdersByCustomerQueryHandler> logger, IMapper mapper, IOrderRepository orderRepository)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.orderRepository = orderRepository;
        }

        public Task<Result<IEnumerable<OrderDto>>> Handle(GetOrdersByCustomerQuery query, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
