using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Catalog.Application.Features.Product.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; private set; }
        public string Description { get; set; }
        public string SKU { get; set; }
        public int AmountInStock { get; set; }
        public int Price { get; set; }
        public string Currency { get; set; }
        public int MinStock { get; set; }        
    }
}
