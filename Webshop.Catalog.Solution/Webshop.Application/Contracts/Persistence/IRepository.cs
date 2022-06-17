using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Domain.Common;

namespace Webshop.Application.Contracts.Persistence
{
    public interface IRepository<T> where T : AggregateRoot
    {
        Task CreateAsync(T entity);
        Task DeleteAsync(T entity);
        T GetById(int id);
        IEnumerable<T> GetAll();
    }
}
