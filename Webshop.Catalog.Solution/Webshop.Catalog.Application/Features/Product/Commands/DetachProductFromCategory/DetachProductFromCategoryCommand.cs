using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;

namespace Webshop.Catalog.Application.Features.Product.Commands.DetachProductFromCategory
{
    public class DetachProductFromCategoryCommand : ICommand
    {
        public DetachProductFromCategoryCommand(int productId, int categoryId)
        {
            ProductId = productId;
            CategoryId = categoryId;
        }

        public int ProductId { get; private set; }
        public int CategoryId { get; private set; }
    }
}
