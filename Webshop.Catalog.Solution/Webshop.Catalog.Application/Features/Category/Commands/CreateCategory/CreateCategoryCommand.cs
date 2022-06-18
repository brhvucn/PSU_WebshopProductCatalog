using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;

namespace Webshop.Category.Application.Features.Category.Commands.CreateCategory
{
    public class CreateCategoryCommand : ICommand
    {
        public CreateCategoryCommand(string name, string description, int parentId)
        {
            Name = name;
            Description = description;
            this.ParentId = parentId;
        }

        public string Name { get; private set; }
        public int ParentId { get; set; }
        public string Description { get; private set; }
    }
}
