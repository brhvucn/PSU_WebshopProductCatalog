using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Order.Application.Features.Order.Dtos;

namespace Webshop.Order.Application.Features.Order.Queries.GetOrders
{
    public class GetOrdersByCustomer : IQuery<OrderDto>
    {
        public GetOrdersByCustomer(int orderId)
        {
            OrderId  = orderId;
        }

        public int OrderId { get; set; }
    }
}
