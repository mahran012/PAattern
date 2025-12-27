using SAP.NET6.Data;
using System.IO;
using System.Text.Json;

namespace SAP.NET6.Services.Catalog.FileProcessing
{
    public class JsonCatalogFileGenerator : CatalogFileGenerator
    {
        public JsonCatalogFileGenerator(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public override void GenerateCatalogFile(string filePath)
        {
            var catalogItems = GetCatalogItemsForExport();
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            
            var jsonString = JsonSerializer.Serialize(catalogItems, options);
            File.WriteAllText(filePath, jsonString);
        }

        public override bool CanGenerate(string fileName)
        {
            return fileName.ToLower().EndsWith(".json");
        }
    }
}