using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Domain.AggregateRoots;

namespace Webshop.Order.Application.Features.Order.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public Customer Customer { get; set; }
        public DateTime DateOfIssue { get; set; }
        public DateTime DueDate { get; set; }
        public int Discount { get; set; }
        public Dictionary<Catalog.Domain.AggregateRoots.Product, int> OrderedProducts { get; set; }
    }
}
