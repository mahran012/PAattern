using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SAP.NET6.ViewModels.Catalog.Admin
{
    public class CreateCategoryViewModel
    {
        public Guid? ParentId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
