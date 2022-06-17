using EnsureThat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;

namespace Webshop.Customer.Application.Features.CreateCustomer
{
    public class CreateCustomerCommand : ICommand
    {
        public CreateCustomerCommand(Domain.AggregateRoots.Customer customer)
        {
            Ensure.That(customer, nameof(customer)).IsNotNull();
            Customer = customer;
        }

        public Domain.AggregateRoots.Customer Customer { get; private set; }
    }
}
