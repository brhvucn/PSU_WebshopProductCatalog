using EnsureThat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Domain.AggregateRoots;
using EnsureThat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Catalog.Domain.AggregateRoots;
using Webshop.Domain.Common;

namespace Webshop.Order.Domain.AggregateRoots
{
    public class Order : AggregateRoot
    {
        public Order(Customer customer, DateTime dateOfIssue, DateTime dueDate, Dictionary<Product, int> orderedProducts)
        {
            Ensure.That(dateOfIssue != DateTime.MinValue);
            DateOfIssue = dateOfIssue;

            Ensure.That(dueDate != DateTime.MinValue);
            DueDate = dueDate;

            Ensure.That(customer != null);
            Customer = customer;

            Ensure.That(OrderedProducts != null);
            OrderedProducts = orderedProducts;
        }

        public Order() { } //for ORM

        public Customer Customer { get; set; }
        public DateTime DateOfIssue { get; set; }
        public DateTime DueDate { get; set; }
        public int Discount { get; set; }
        public Dictionary<Catalog.Domain.AggregateRoots.Product, int> OrderedProducts { get; set; }
    }


}
