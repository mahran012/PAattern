using SAP.NET6.Services.Catalog.FileProcessing;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SAP.NET6.Services
{
    public class DistributorApiClient : IDistributorApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://sap.thevvp.ru/";

        public DistributorApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CatalogItemFileModel>> GetCatalogAsync()
        {
            try
            {
                var response = await _httpClient.GetStringAsync($"{_baseUrl}api/catalog");
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                
                return JsonSerializer.Deserialize<List<CatalogItemFileModel>>(response, options) ?? new List<CatalogItemFileModel>();
            }
            catch (Exception ex)
            {
                // In a real implementation, proper logging would be used
                throw new Exception($"Error retrieving catalog from distributor API: {ex.Message}", ex);
            }
        }

        public async Task NotifyOrderAsync(Dictionary<string, int> orderedItems)
        {
            try
            {
                var json = JsonSerializer.Serialize(orderedItems);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync($"{_baseUrl}api/orders", content);
                
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to notify distributor about order. Status: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                // In a real implementation, proper logging would be used
                throw new Exception($"Error notifying distributor about order: {ex.Message}", ex);
            }
        }
    }
}