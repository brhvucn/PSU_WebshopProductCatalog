using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;

namespace Webshop.Catalog.Application.Features.Product.Commands.AttachProductToCategory
{
    public class AttachProductToCategoryCommand : ICommand
    {
        public AttachProductToCategoryCommand(int productId, int categoryId)
        {
            ProductId = productId;
            CategoryId = categoryId;
        }

        public int ProductId { get; private set; }
        public int CategoryId { get; private set; }        
    }
}
