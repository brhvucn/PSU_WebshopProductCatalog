using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;

namespace Webshop.Catalog.Application.Features.Product.Commands.UpdateProduct
{
    public class UpdateProductCommand : ICommand
    {
        public UpdateProductCommand(int id, string name, string description, string sKU, int amountInStock, int price, string currency, int minStock)
        {
            Id = id;
            Name = name;
            Description = description;
            SKU = sKU;
            AmountInStock = amountInStock;
            Price = price;
            Currency = currency;
            MinStock = minStock;
        }
    
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string SKU { get; private set; }
        public int AmountInStock { get; private set; }
        public int Price { get; private set; }
        public string Currency { get; private set; }
        public int MinStock { get; private set; }        
    }
}
