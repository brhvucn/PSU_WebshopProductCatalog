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
using Webshop.Order.Application.Features.Order.Queries.GetOrder;

namespace Webshop.Order.Application.Features.Order.Queries.GetOrder
{
    public class GetOrderQueryHandler : IQueryHandler<GetOrderQuery, OrderDto>
    {
        private ILogger<GetOrderQuery> logger;
        private IMapper mapper;
        private IOrderRepository orderRepository;

        public GetOrderQueryHandler(ILogger<GetOrderQuery> logger, IMapper mapper, IOrderRepository orderRepository)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.orderRepository = orderRepository;
        }

        public async Task<Result<OrderDto>> Handle(GetOrderQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                var order = await this.orderRepository.GetAsync(query.OrderId);
                return this.mapper.Map<OrderDto>(order);
            }
            catch (Exception ex)
            {
                this.logger.LogCritical(ex, ex.Message);
                return Result.Fail<OrderDto>(Errors.General.UnspecifiedError(ex.Message));
            }
        }
    }
}
