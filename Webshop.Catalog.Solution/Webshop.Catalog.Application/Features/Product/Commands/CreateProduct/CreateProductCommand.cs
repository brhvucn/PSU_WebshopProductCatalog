using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;

namespace Webshop.Catalog.Application.Features.Product.Commands.CreateProduct
{
    public class CreateProductCommand : ICommand
    {
        public CreateProductCommand(string name, string sKU, int price, string currency)
        {
            Name = name;
            SKU = sKU;
            Price = price;
            Currency = currency;
        }

        public string Name { get; private set; }
        public string SKU { get; private set; }
        public int Price { get; private set; }
        public string Currency { get; private set; }        
    }
}
