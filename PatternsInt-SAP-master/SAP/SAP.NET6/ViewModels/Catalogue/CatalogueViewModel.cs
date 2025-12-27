using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAP.NET6.ViewModels.Catalog
{
    public class CatalogViewModel
    {
        public List<CategoryViewModel> RootCategories { get; set; }

        public List<ItemViewModel> Items { get; set; }
    }
}
