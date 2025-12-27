using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SAP.NET6.Data.Models.Catalog
{
    public abstract class Node
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
