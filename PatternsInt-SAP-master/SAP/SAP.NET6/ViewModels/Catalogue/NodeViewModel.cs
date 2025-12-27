using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SAP.NET6.ViewModels.Catalog
{
    public abstract class NodeViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
