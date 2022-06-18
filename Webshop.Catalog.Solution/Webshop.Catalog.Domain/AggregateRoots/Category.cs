using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Domain.Common;

namespace Webshop.Catalog.Domain.AggregateRoots
{
    public class Category : AggregateRoot
    {
        public Category(string name)
        {
            Name = name;
            ChildCategories = new List<Category>();
        }

        public Category() { } //for ORM
        public string Name { get; private set; }
        public string Description { get; set; }
        public IEnumerable<Category> ChildCategories { get; set; }
        public int ParentId { get; set; }
    }
}
