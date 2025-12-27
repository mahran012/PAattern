namespace SAP.NET6.Services.Catalog.FileProcessing
{
    public interface ICatalogFileGenerator
    {
        void GenerateCatalogFile(string filePath);
        bool CanGenerate(string fileName);
    }
}