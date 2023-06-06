using EnsureThat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Domain.AggregateRoots;

namespace Webshop.Order.Application.Features.Order.Commands.CreateOrder
{
    public class CreateOrderCommand : ICommand
    {
        public CreateOrderCommand(Customer customer, DateTime dateOfIssue, DateTime dueDate, int discount, Dictionary<Catalog.Domain.AggregateRoots.Product, int> orderProducts)
        {
            Ensure.That(customer, nameof(customer)).IsNotNull();
            Customer = customer;
            Ensure.That(dateOfIssue, nameof(dateOfIssue)).IsNot(DateTime.MinValue);
            DateOfIssue = dateOfIssue;
            Ensure.That(dueDate, nameof(dueDate)).IsNot(DateTime.MinValue);
            DueDate = dueDate;
            Ensure.That(orderProducts, nameof(orderProducts)).IsNotNull();
            Ensure.That(orderProducts, nameof(orderProducts)).HasItems();
            OrderProducts = orderProducts;
            Ensure.That(discount, nameof(discount)).IsInRange(0,15);
            Discount = discount;
        }

        public Customer Customer { get; set; }
        public DateTime DateOfIssue { get; set; }
        public DateTime DueDate { get; set; }
        public int Discount { get; set; }
        public Dictionary<Catalog.Domain.AggregateRoots.Product, int> OrderProducts { get; set; }
    }
}
