using SAP.NET6.Data;
using SAP.NET6.Data.Models.Catalog;
using SAP.NET6.Services.Catalog.Implementations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SAP.NET6.Services.Catalog.FileProcessing
{
    public class CatalogFileProcessor
    {
        private readonly List<IFileParser> _parsers;
        private readonly ApplicationDbContext _dbContext;

        public CatalogFileProcessor(ApplicationDbContext dbContext)
        {
            _parsers = new List<IFileParser>
            {
                new JsonFileParser(),
                new XmlFileParser()
            };
            _dbContext = dbContext;
        }

        public void ProcessCatalogFile(string filePath)
        {
            var fileName = Path.GetFileName(filePath);
            
            // Find the appropriate parser
            var parser = _parsers.FirstOrDefault(p => p.CanParse(fileName));
            
            if (parser == null)
            {
                throw new InvalidOperationException($"No parser available for file: {fileName}");
            }

            // Parse the file
            var catalogItems = parser.Parse(filePath);
            
            // Update the catalog with the parsed items
            UpdateCatalogFromItems(catalogItems);
        }

        private void UpdateCatalogFromItems(List<CatalogItemFileModel> catalogItems)
        {
            foreach (var itemFileModel in catalogItems)
            {
                // Find or create the category
                var category = _dbContext.Categories.FirstOrDefault(c => c.Name == itemFileModel.CategoryName);
                if (category == null)
                {
                    category = new Category
                    {
                        Name = itemFileModel.CategoryName,
                        Childs = new List<Category>()
                    };
                    _dbContext.Categories.Add(category);
                    _dbContext.SaveChanges();
                }

                // Check if item already exists by ManufacturerId
                var existingItem = _dbContext.Items.FirstOrDefault(i => i.ManufacturerId == itemFileModel.ManufacturerId);
                
                if (existingItem != null)
                {
                    // Update existing item
                    existingItem.Name = itemFileModel.Name;
                    existingItem.CategoryId = category.Id;
                    existingItem.QuantityInStock = itemFileModel.QuantityInStock;
                    existingItem.PictureUrl = itemFileModel.PictureUrl;
                    
                    // Update attributes
                    _dbContext.Attributes.RemoveRange(_dbContext.Attributes.Where(a => a.ItemId == existingItem.Id));
                    _dbContext.SaveChanges();
                    
                    foreach (var attrFileModel in itemFileModel.Attributes)
                    {
                        var color = Enum.TryParse<Colors>(attrFileModel.Color, true, out var parsedColor) ? parsedColor : Colors.Black;
                        var attribute = new Attributes
                        {
                            Color = color,
                            Width = attrFileModel.Width,
                            Height = attrFileModel.Height,
                            Length = attrFileModel.Length,
                            Weight = attrFileModel.Weight,
                            Price = attrFileModel.Price,
                            ItemId = existingItem.Id
                        };
                        _dbContext.Attributes.Add(attribute);
                    }
                }
                else
                {
                    // Create new item
                    var newItem = new Item
                    {
                        Name = itemFileModel.Name,
                        ManufacturerId = itemFileModel.ManufacturerId,
                        CategoryId = category.Id,
                        QuantityInStock = itemFileModel.QuantityInStock,
                        PictureUrl = itemFileModel.PictureUrl,
                        Attributes = new List<Attributes>()
                    };
                    
                    _dbContext.Items.Add(newItem);
                    _dbContext.SaveChanges(); // Save to get the item ID
                    
                    foreach (var attrFileModel in itemFileModel.Attributes)
                    {
                        var color = Enum.TryParse<Colors>(attrFileModel.Color, true, out var parsedColor) ? parsedColor : Colors.Black;
                        var attribute = new Attributes
                        {
                            Color = color,
                            Width = attrFileModel.Width,
                            Height = attrFileModel.Height,
                            Length = attrFileModel.Length,
                            Weight = attrFileModel.Weight,
                            Price = attrFileModel.Price,
                            ItemId = newItem.Id
                        };
                        _dbContext.Attributes.Add(attribute);
                    }
                }
            }
            
            _dbContext.SaveChanges();
        }
    }
}