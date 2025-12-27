using SAP.NET6.Data;
using SAP.NET6.Data.Models;
using SAP.NET6.Data.Models.Catalog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SAP.NET6.Services.Catalog.FileProcessing
{
    public class CatalogWithOrdersFileService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly CatalogFileGeneratorFactory _factory;

        public CatalogWithOrdersFileService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _factory = new CatalogFileGeneratorFactory(dbContext);
        }

        public void GenerateCatalogWithOrdersFile(string filePath)
        {
            // Get the catalog items
            var catalogItems = GetCatalogItemsWithOrderCounts();
            
            // Create the appropriate generator based on file extension
            var generator = _factory.CreateGenerator(filePath);
            
            // For this implementation, we'll create a custom generator that can handle order counts
            // Since the existing generators don't handle order counts, we'll need to extend them
            // For now, we'll implement the functionality directly
            var fileName = Path.GetFileName(filePath);
            
            if (fileName.ToLower().EndsWith(".json"))
            {
                GenerateJsonWithOrderCounts(catalogItems, filePath);
            }
            else if (fileName.ToLower().EndsWith(".xml"))
            {
                GenerateXmlWithOrderCounts(catalogItems, filePath);
            }
            else
            {
                throw new InvalidOperationException($"Unsupported file format: {fileName}");
            }
        }

        private List<CatalogItemWithOrders> GetCatalogItemsWithOrderCounts()
        {
            var items = _dbContext.Items.ToList();
            var catalogItems = new List<CatalogItemWithOrders>();

            foreach (var item in items)
            {
                var category = _dbContext.Categories.FirstOrDefault(c => c.Id == item.CategoryId);
                
                // Count how many times this item was ordered
                var orderCount = _dbContext.ItemToOrders.Count(i => i.ItemId == item.Id);
                
                var catalogItem = new CatalogItemWithOrders
                {
                    Name = item.Name,
                    ManufacturerId = item.ManufacturerId,
                    CategoryName = category?.Name ?? "Unknown",
                    QuantityInStock = item.QuantityInStock,
                    PictureUrl = item.PictureUrl,
                    OrdersCount = orderCount, // Number of times ordered
                    Attributes = new List<AttributeFileModel>()
                };

                foreach (var attr in item.Attributes)
                {
                    catalogItem.Attributes.Add(new AttributeFileModel
                    {
                        Color = attr.Color.ToString(),
                        Width = attr.Width,
                        Height = attr.Height,
                        Length = attr.Length,
                        Weight = attr.Weight,
                        Price = attr.Price
                    });
                }

                catalogItems.Add(catalogItem);
            }

            return catalogItems;
        }

        private void GenerateJsonWithOrderCounts(List<CatalogItemWithOrders> catalogItems, string filePath)
        {
            var options = new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true
            };
            
            var jsonString = System.Text.Json.JsonSerializer.Serialize(catalogItems, options);
            File.WriteAllText(filePath, jsonString);
        }

        private void GenerateXmlWithOrderCounts(List<CatalogItemWithOrders> catalogItems, string filePath)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<CatalogItemWithOrders>));
            
            using var fileStream = new FileStream(filePath, FileMode.Create);
            serializer.Serialize(fileStream, catalogItems);
        }
    }

    // Extended model that includes order count
    [System.Xml.Serialization.XmlRoot("CatalogItems")]
    public class CatalogItemWithOrders : CatalogItemFileModel
    {
        [System.Xml.Serialization.XmlElement("OrdersCount")]
        public int OrdersCount { get; set; }
    }
}