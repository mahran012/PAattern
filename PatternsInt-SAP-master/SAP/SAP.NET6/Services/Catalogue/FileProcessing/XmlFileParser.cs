using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SAP.NET6.Services.Catalog.FileProcessing
{
    public class XmlFileParser : IFileParser
    {
        public List<CatalogItemFileModel> Parse(string filePath)
        {
            // Try to deserialize as a list first
            try
            {
                var serializer = new XmlSerializer(typeof(List<CatalogItemFileModel>));
                
                using var fileStream = new FileStream(filePath, FileMode.Open);
                var catalogItems = serializer.Deserialize(fileStream) as List<CatalogItemFileModel>;
                
                return catalogItems ?? new List<CatalogItemFileModel>();
            }
            catch (InvalidOperationException)
            {
                // If the above fails, try to deserialize as a wrapper object
                var wrapperSerializer = new XmlSerializer(typeof(CatalogItemsWrapper));
                
                using var fileStream = new FileStream(filePath, FileMode.Open);
                var wrapper = wrapperSerializer.Deserialize(fileStream) as CatalogItemsWrapper;
                
                return wrapper?.CatalogItems ?? new List<CatalogItemFileModel>();
            }
        }

        public bool CanParse(string fileName)
        {
            return fileName.ToLower().EndsWith(".xml");
        }
    }
}