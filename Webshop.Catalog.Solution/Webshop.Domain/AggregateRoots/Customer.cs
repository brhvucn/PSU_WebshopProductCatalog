using EnsureThat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Domain.Common;

namespace Webshop.Domain.AggregateRoots
{
    public class Customer : AggregateRoot
    {
        public Customer(string name)
        {
            Ensure.That(name, nameof(name)).IsNotNullOrEmpty();
            Name = name;
        }

        public Customer() { } //for ORM

        public string Name { get; private set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
    }
}
