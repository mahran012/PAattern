using System.Collections.Generic;

namespace SAP.NET6.Services.Catalog.FileProcessing
{
    public interface IFileParser
    {
        List<CatalogItemFileModel> Parse(string filePath);
        bool CanParse(string fileName);
    }
}