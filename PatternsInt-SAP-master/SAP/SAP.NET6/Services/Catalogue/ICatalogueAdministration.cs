using SAP.NET6.ViewModels.Catalog;
using SAP.NET6.ViewModels.Catalog.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAP.NET6.Services.Catalog
{
    public interface ICatalogAdministration
    {
        Task CreateCategoryAsync(CreateCategoryViewModel category);

        Task CreateItemAsync(CreateItemViewModel item);

        Task DeleteItemAsync(Guid id);
    }
}
