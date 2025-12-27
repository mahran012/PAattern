using SAP.NET6.Data;
using System.IO;
using System.Xml.Serialization;

namespace SAP.NET6.Services.Catalog.FileProcessing
{
    public class XmlCatalogFileGenerator : CatalogFileGenerator
    {
        public XmlCatalogFileGenerator(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public override void GenerateCatalogFile(string filePath)
        {
            var catalogItems = GetCatalogItemsForExport();
            var serializer = new XmlSerializer(typeof(List<CatalogItemFileModel>));
            
            using var fileStream = new FileStream(filePath, FileMode.Create);
            serializer.Serialize(fileStream, catalogItems);
        }

        public override bool CanGenerate(string fileName)
        {
            return fileName.ToLower().EndsWith(".xml");
        }
    }
}