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
            Name = name;
        }

        public string Name { get; private set; }
    }
}
