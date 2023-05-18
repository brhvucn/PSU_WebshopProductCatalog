using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Domain.Common;
using static Webshop.Data.Persistence.TableNames;

namespace Webshop.Domain.AggregateRoots
{
    public class Order : AggregateRoot
    {
        public Order()
        {
            /*Ensure.That(name, nameof(name)).IsNotNullOrEmpty();
            Name = name;*/
        }

        public Order() { } //for ORM

        public Customer Customer { get; private set; }
        public DateTime DateOfIssue { get; set; }
        public DateTime DueDate { get; set; }
        public IEnumerable<KeyValuePair<Webshop., int>> OrderedItems { get; set; }

    }
}
