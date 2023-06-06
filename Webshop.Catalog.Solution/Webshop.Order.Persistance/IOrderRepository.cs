using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts.Persistence;

namespace Webshop.Order.Persistance
{
    public interface IOrderRepository : IRepository<Domain.AggregateRoots.Order>
    {
    }
}
