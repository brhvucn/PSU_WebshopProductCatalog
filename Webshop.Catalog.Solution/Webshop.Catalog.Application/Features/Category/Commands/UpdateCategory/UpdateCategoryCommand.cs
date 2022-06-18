using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;

namespace Webshop.Category.Application.Features.Category.Commands.UpdateCategory
{
    public class UpdateCategoryCommand : ICommand
    {
        public UpdateCategoryCommand(string name, string description, int id)
        {
            Name = name;
            Description = description;
            Id = id;
        }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Id { get; private set; }
    }
}
