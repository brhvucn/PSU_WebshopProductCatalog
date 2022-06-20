using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts.Persistence;
using Webshop.Catalog.Domain.AggregateRoots;
using Webshop.Domain.Common;

namespace Webshop.Catalog.Application.Contracts.Persistence
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetAllFromCategory(int categoryId);
        Task<Result> AddProductToCategory(int productId, int categoryId);
        Task<Result> RemoveProductFromCategory(int productId, int categoryId);
    }
}
