using SAP.NET6.ViewModels.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAP.NET6.ViewModels.Orders
{
    public class OrderItemViewModel
    {
        public Guid ItemId { get; set; }

        public ItemViewModel Item { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public AttributesViewModel Attributes { get; set; }
    }
}
