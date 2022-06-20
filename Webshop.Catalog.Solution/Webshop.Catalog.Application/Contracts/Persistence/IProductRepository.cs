using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts.Persistence;
using Webshop.Catalog.Domain.AggregateRoots;

namespace Webshop.Catalog.Application.Contracts.Persistence
{
    public interface IProductRepository : IRepository<Product>
    {
    }
}
