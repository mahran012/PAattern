using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAP.NET6.ViewModels.Catalog
{
    public class CategoryViewModel : NodeViewModel
    {
        public bool IsSelected { get; set; }

        public Guid? ParentId { get; set; }

        public CategoryViewModel Parent { get; set; }

        public List<CategoryViewModel> Childs { get; set; }

        public List<ItemViewModel> Items { get; set; }
    }
}
