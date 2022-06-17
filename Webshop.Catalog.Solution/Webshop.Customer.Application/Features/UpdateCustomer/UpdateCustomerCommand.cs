using EnsureThat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;

namespace Webshop.Customer.Application.Features.UpdateCustomer
{
    public class UpdateCustomerCommand : ICommand
    {
        public UpdateCustomerCommand(Domain.AggregateRoots.Customer customer)
        {
            Ensure.That(customer, nameof(customer)).IsNotNull();
            Ensure.That(customer.Id, nameof(customer.Id)).IsNotDefault();
            Ensure.That(customer.Id, nameof(customer.Id)).IsGt<int>(0);
            Ensure.That(customer.Name, nameof(customer.Name)).IsNotNullOrEmpty();
            Customer = customer;
        }

        public Domain.AggregateRoots.Customer Customer { get; private set; }
    }
}
