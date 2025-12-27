using SAP.NET6.Data.Models.Catalog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SAP.NET6.ViewModels.Catalog
{
    [DataContract]
    public class AttributesViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public Colors Color { get; set; }

        [Required]
        public double Width { get; set; }

        [Required]
        public double Height { get; set; }

        [Required]
        public double Length { get; set; }

        [Required]
        public double Weight { get; set; }

        [DataMember]
        [Required]
        public decimal Price { get; set; }

        public string ViewData => $"{Color.ToString()} {Price}$";
    }
}
