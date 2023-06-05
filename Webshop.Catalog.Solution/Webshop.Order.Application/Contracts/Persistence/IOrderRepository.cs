using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Domain.Common;

namespace Webshop.Order.Application.Contracts.Persistence
{
    public interface IOrderRepository
    {
        Task<Result> CreateAsync(Domain.AggregateRoots.Order entity);

        Task<Domain.AggregateRoots.Order> GetAsync(int id);

        Task<IEnumerable<Domain.AggregateRoots.Order>> GetAllAsync();

        Task<int> DeleteAsync(int id);
    }
}
