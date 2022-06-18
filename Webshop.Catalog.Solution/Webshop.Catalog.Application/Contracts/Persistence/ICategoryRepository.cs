using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts.Persistence;
using Webshop.Catalog.Application.Features.Catalog.Dtos;
using Webshop.Catalog.Domain.AggregateRoots;

namespace Webshop.Catalog.Application.Contracts.Persistence
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IEnumerable<Category>> GetRootCategories();
        Task<IEnumerable<Category>> GetChildCategories(int parentCategory);
        Task<bool> ExistsCategory(int parentId);
    }
}
