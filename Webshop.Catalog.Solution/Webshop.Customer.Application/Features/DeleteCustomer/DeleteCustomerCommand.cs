using EnsureThat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;

namespace Webshop.Customer.Application.Features.DeleteCustomer
{
    public class DeleteCustomerCommand : ICommand
    {
        public DeleteCustomerCommand(int customerId)
        {
            Ensure.That(customerId, nameof(customerId)).IsNotDefault<int>(); //no default, or zero
            Ensure.That(customerId, nameof(customerId)).IsGt<int>(0); //no negative id
            CustomerId = customerId;
        }

        public int CustomerId { get; private set; }
    }
}
