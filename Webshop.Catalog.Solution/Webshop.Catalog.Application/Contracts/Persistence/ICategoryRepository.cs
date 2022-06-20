using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts.Persistence;
using Webshop.Domain.Common;

namespace Webshop.Catalog.Application.Contracts.Persistence
{
    public interface ICategoryRepository : IRepository<Domain.AggregateRoots.Category>
    {
        Task<IEnumerable<Domain.AggregateRoots.Category>> GetRootCategories();
        Task<IEnumerable<Domain.AggregateRoots.Category>> GetChildCategories(int parentCategory);
        Task<bool> ExistsCategory(int parentId);
        Task<IEnumerable<Domain.AggregateRoots.Category>> GetForProduct(int productId);
    }
}
