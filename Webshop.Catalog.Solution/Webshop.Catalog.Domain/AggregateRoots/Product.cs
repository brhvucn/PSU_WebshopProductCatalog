using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Domain.Common;

namespace Webshop.Catalog.Domain.AggregateRoots
{
    public class Product : AggregateRoot
    {
        public Product(string name, string sku, int price, string currency)
        {
            Name = name;
            SKU = sku;
            Price = price;
            Currency = currency;
        }

        public Product() { } //for ORM
        public string Name { get; private set; }
        public string Description { get; set; }
        /// <summary>
        /// Stock keeping unit
        /// </summary>
        public string SKU { get; set; }
        public int AmountInStock { get; set; }

        /// <summary>
        /// The price is represented in the lowest monetary value. For Euros this is cents, this means that to show the price on the web site, we should divide with 100
        /// </summary>
        public int Price { get; set; } 
        public string Currency { get; set; } 
        public int MinStock { get; set; }        
    }
}
