using SAP.NET6.Data.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAP.NET6.Data.Models
{
    public class ItemToOrder
    {
        public Guid OrderId { get; set; }

        public virtual Order Order { get; set; }

        public Guid ItemId { get; set; }

        public Item Item { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public int AttributesId { get; set; }

        public Attributes Attributes { get; set; }
    }
}
