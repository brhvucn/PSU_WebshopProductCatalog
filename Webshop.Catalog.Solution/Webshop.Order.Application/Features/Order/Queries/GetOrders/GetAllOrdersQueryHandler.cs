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

namespace Webshop.Order.Application.Features.Order.Queries.GetOrders
{
    public class GetAllOrdersQueryHandler : IQueryHandler<GetAllOrdersQuery, IEnumerable<OrderDto>>
    {
        private ILogger<GetAllOrdersQueryHandler> logger;
        private IMapper mapper;
        private IOrderRepository orderRepository;

        public GetAllOrdersQueryHandler(ILogger<GetAllOrdersQueryHandler> logger, IMapper mapper, IOrderRepository orderRepository)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.orderRepository = orderRepository;
        }

        public async Task<Result<IEnumerable<OrderDto>>> Handle(GetAllOrdersQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                IEnumerable<Domain.AggregateRoots.Order> orders = await this.orderRepository.GetAllAsync();
                return Result.Ok(this.mapper.Map<IEnumerable<OrderDto>>(orders));
            }
            catch (Exception ex)
            {
                this.logger.LogCritical(ex, ex.Message);
                return Result.Fail<IEnumerable<OrderDto>>(Errors.General.UnspecifiedError(ex.Message));
            }
        }
    }
}
