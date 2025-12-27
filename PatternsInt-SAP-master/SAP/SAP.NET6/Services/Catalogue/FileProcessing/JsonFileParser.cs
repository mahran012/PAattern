using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SAP.NET6.Services.Catalog.FileProcessing
{
    public class JsonFileParser : IFileParser
    {
        public List<CatalogItemFileModel> Parse(string filePath)
        {
            var jsonString = File.ReadAllText(filePath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            
            var catalogItems = JsonSerializer.Deserialize<List<CatalogItemFileModel>>(jsonString, options);
            return catalogItems ?? new List<CatalogItemFileModel>();
        }

        public bool CanParse(string fileName)
        {
            return fileName.ToLower().EndsWith(".json");
        }
    }
}