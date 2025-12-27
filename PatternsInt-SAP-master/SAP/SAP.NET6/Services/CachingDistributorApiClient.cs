using SAP.NET6.Services.Catalog.FileProcessing;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SAP.NET6.Services
{
    public class CachingDistributorApiClient : IDistributorApiClient
    {
        private readonly IDistributorApiClient _distributorApiClient;
        private List<CatalogItemFileModel> _cachedCatalog;
        private DateTime _cacheExpiryTime;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(5); // Cache for 5 minutes

        public CachingDistributorApiClient(IDistributorApiClient distributorApiClient)
        {
            _distributorApiClient = distributorApiClient;
        }

        public async Task<List<CatalogItemFileModel>> GetCatalogAsync()
        {
            // Check if cache is valid
            if (_cachedCatalog != null && DateTime.UtcNow < _cacheExpiryTime)
            {
                // Return cached data
                return _cachedCatalog;
            }

            // Cache is expired or doesn't exist, fetch fresh data
            _cachedCatalog = await _distributorApiClient.GetCatalogAsync();
            _cacheExpiryTime = DateTime.UtcNow.Add(_cacheDuration);

            return _cachedCatalog;
        }

        public async Task NotifyOrderAsync(Dictionary<string, int> orderedItems)
        {
            // Forward the notification to the actual API client
            await _distributorApiClient.NotifyOrderAsync(orderedItems);
            
            // Optionally invalidate the cache after an order notification
            // This might be desired if the distributor updates inventory after orders
            // _cachedCatalog = null; 
        }
    }
}