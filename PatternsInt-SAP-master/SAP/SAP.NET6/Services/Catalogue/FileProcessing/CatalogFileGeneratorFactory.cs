using SAP.NET6.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAP.NET6.Services.Catalog.FileProcessing
{
    public class CatalogFileGeneratorFactory
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly List<ICatalogFileGenerator> _generators;

        public CatalogFileGeneratorFactory(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _generators = new List<ICatalogFileGenerator>
            {
                new JsonCatalogFileGenerator(_dbContext),
                new XmlCatalogFileGenerator(_dbContext)
            };
        }

        public ICatalogFileGenerator CreateGenerator(string fileName)
        {
            var generator = _generators.FirstOrDefault(g => g.CanGenerate(fileName));
            if (generator == null)
            {
                throw new InvalidOperationException($"No generator available for file: {fileName}");
            }
            return generator;
        }
    }
}