using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Domain.AggregateRoots;

namespace Webshop.Order.Application.Features.Category.Commands.CreateOrder
{
    public class CreateOrderCommand : ICommand
    {
        public CreateOrderCommand(Customer customer, DateTime dateOfIssue, DateTime dueDate, Dictionary<Catalog.Domain.AggregateRoots.Product, int> orderProducts)
        {
            Customer = customer;
            DateOfIssue = dateOfIssue;
            DueDate = dueDate;
            OrderProducts = orderProducts;
        }

        public Customer Customer { get; set; }
        public DateTime DateOfIssue { get; set; }
        public DateTime DueDate { get; set; }
        public Dictionary<Catalog.Domain.AggregateRoots.Product, int> OrderProducts { get; set; }
    }
}
