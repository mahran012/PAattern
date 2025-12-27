using SAP.NET6.Data;
using SAP.NET6.Data.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAP.NET6.Services.Catalog.FileProcessing
{
    public abstract class CatalogFileGenerator : ICatalogFileGenerator
    {
        protected ApplicationDbContext DbContext { get; }

        public CatalogFileGenerator(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public abstract void GenerateCatalogFile(string filePath);
        public abstract bool CanGenerate(string fileName);

        protected List<CatalogItemFileModel> GetCatalogItemsForExport()
        {
            var items = DbContext.Items.ToList();
            var catalogItems = new List<CatalogItemFileModel>();

            foreach (var item in items)
            {
                var category = DbContext.Categories.FirstOrDefault(c => c.Id == item.CategoryId);
                
                var catalogItem = new CatalogItemFileModel
                {
                    Name = item.Name,
                    ManufacturerId = item.ManufacturerId,
                    CategoryName = category?.Name ?? "Unknown",
                    QuantityInStock = item.QuantityInStock,
                    PictureUrl = item.PictureUrl,
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
    }
}