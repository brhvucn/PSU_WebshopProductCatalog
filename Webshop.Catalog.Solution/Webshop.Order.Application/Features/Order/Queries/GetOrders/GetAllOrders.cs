using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Order.Application.Features.Category.Dtos;

namespace Webshop.Order.Application.Features.Order.Queries.GetOrders
{
    internal class GetAllOrdersQuery : IQuery<IEnumerable<OrderDto>>
    {
    }
}
