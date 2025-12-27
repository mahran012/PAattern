using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SAP.NET6.Services.Catalog.FileProcessing
{
    public class CatalogItemFileModel
    {
        [XmlElement("Name")]
        public string Name { get; set; }
        
        [XmlElement("ManufacturerId")]
        public string ManufacturerId { get; set; }
        
        [XmlElement("CategoryName")]
        public string CategoryName { get; set; }
        
        [XmlElement("QuantityInStock")]
        public int QuantityInStock { get; set; }
        
        [XmlElement("PictureUrl")]
        public string PictureUrl { get; set; }
        
        [XmlArray("Attributes")]
        [XmlArrayItem("Attribute")]
        public List<AttributeFileModel> Attributes { get; set; } = new List<AttributeFileModel>();
    }

    public class AttributeFileModel
    {
        [XmlElement("Color")]
        public string Color { get; set; }
        
        [XmlElement("Width")]
        public double Width { get; set; }
        
        [XmlElement("Height")]
        public double Height { get; set; }
        
        [XmlElement("Length")]
        public double Length { get; set; }
        
        [XmlElement("Weight")]
        public double Weight { get; set; }
        
        [XmlElement("Price")]
        public decimal Price { get; set; }
    }

    [XmlRoot("CatalogItems")]
    public class CatalogItemsWrapper
    {
        [XmlElement("CatalogItem")]
        public List<CatalogItemFileModel> CatalogItems { get; set; } = new List<CatalogItemFileModel>();
    }
}