using EnsureThat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Customer.Application.Features.Dto;

namespace Webshop.Customer.Application.Features.GetCustomer
{
    public class GetCustomerQuery : IQueryHandler<CustomerDto>
    {
        public GetCustomerQuery(int customerId)
        {
            Ensure.That(customerId, nameof(customerId)).IsNotDefault<int>();
            Ensure.That(customerId, nameof(customerId)).IsGt<int>(0);
            CustomerId = customerId;
        }

        public int CustomerId { get; private set; }
    }
}
