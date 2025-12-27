using System.Collections.Generic;
using System.Threading.Tasks;

namespace SAP.NET6.Services
{
    public interface IDistributorApiClient
    {
        Task<List<CatalogItemFileModel>> GetCatalogAsync();
        Task NotifyOrderAsync(Dictionary<string, int> orderedItems);
    }
}