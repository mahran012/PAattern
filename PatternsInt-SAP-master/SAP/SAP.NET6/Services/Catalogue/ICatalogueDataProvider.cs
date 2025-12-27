using SAP.NET6.ViewModels.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAP.NET6.Services.Catalog
{
    public interface ICatalogDataProvider
    {
        Task<CatalogViewModel> GetCatalogAsync(Guid? id = null);

        Task<ItemViewModel> GetItemAsync(Guid id);
    }
}
