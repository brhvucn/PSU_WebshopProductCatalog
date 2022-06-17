using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Customer.Application.Features.Dto;

namespace Webshop.Customer.Application.Features.GetCustomers
{
    public class GetCustomersQuery : IQuery<List<CustomerDto>>
    {
    }
}
