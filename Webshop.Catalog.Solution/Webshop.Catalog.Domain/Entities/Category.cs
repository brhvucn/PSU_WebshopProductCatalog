using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Domain.Common;

namespace Webshop.Catalog.Domain.Entities
{
    public class Category : Entity
    {
        public Category(string name)
        {
            Name = name;
            ChildCategories = new List<Category>();
        }
        public string Name { get; private set; }
        public string Description { get; set; }
        public List<Category> ChildCategories { get; set; }
    }
}
