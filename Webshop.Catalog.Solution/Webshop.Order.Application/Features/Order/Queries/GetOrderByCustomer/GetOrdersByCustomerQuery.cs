using System.Collections.Generic;
using Webshop.Order.Application.Features.Order.Dtos;
using Webshop.Application.Contracts;

namespace Webshop.Order.Application.Features.Order.Queries.GetOrderByCustomer
{
    public class GetOrdersByCustomerQuery : IQueryHandler<IEnumerable<OrderDto>>
    {
        public GetOrdersByCustomerQuery(int customerId)
        {
            CustomerId = customerId;
        }

        public int CustomerId { get; private set; }
    }
}
