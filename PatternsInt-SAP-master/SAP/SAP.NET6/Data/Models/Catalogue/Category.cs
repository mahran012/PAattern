using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SAP.NET6.Data.Models.Catalog
{
    [Table("Categories")]
    public class Category : Node
    {
        public Guid? ParentId { get; set; }

        public virtual Category Parent { get; set; }

        public virtual ICollection<Category> Childs { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }
}
