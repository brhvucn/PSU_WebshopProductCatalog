using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts.Persistence;

namespace Webshop.Customer.Application.Contracts.Persistence
{
    public interface ICustomerRepository : IRepository<Domain.AggregateRoots.Customer>
    {        
    }
}
