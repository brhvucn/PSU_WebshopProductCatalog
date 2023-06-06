using EnsureThat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Order.Application.Features.Order.Dtos;

namespace Webshop.Order.Application.Features.Order.Queries.GetOrder
{
    public class GetOrderQuery : IQueryHandler<OrderDto>
    {
        public GetOrderQuery(int orderId)
        {
            //Smallest possible id is 1
            Ensure.That(orderId, nameof(orderId)).IsGt(0);
            OrderId  = orderId;
        }

        public int OrderId { get; set; }
    }
}
