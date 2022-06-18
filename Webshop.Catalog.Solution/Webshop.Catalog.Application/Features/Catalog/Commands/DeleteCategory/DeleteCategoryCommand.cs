using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;

namespace Webshop.Catalog.Application.Features.Catalog.Commands.DeleteCategory
{
    public class DeleteCategoryCommand : ICommand
    {
        public DeleteCategoryCommand(int categoryId)
        {
            CategoryId = categoryId;
        }

        public int CategoryId { get; private set; }
    }
}
