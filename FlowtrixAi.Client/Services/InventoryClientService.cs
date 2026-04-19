using System.Net.Http.Json;
using FlowtrixAi.Client.Models;

namespace FlowtrixAi.Client.Services;

public class InventoryClientService(HttpClient http)
{
    private const string BaseUrl = "api/Inventory";

    public async Task<List<InventoryResponse>> GetAllInventoryAsync()
    {
        try
        {
            var response = await http.GetFromJsonAsync<List<InventoryResponse>>($"{BaseUrl}/GetAllInventory");
            return response ?? new List<InventoryResponse>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching inventory: {ex.Message}");
            return new List<InventoryResponse>();
        }
    }

    public async Task<bool> AddToInventoryAsync(CreateInventoryRequest request)
    {
        try
        {
            var response = await http.PostAsJsonAsync($"{BaseUrl}/AddToInventory", request);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<WarehouseStats> GetWarehouseStatsAsync()
    {
        var items = await GetAllInventoryAsync();
        
        return new WarehouseStats
        {
            TotalItemsCount = items.Count,
            TotalIncoming = items.Sum(i => i.TotalIncoming),
            TotalUsed = items.Sum(i => i.TotalUsed),
            TotalRemaining = items.Sum(i => i.QuantityAvailable),
            LowStockCount = items.Count(i => i.IsLowStock)
        };
    }

    public async Task<bool> DeleteInventoryAsync(int id)
    {
        try
        {
            var response = await http.DeleteAsync($"{BaseUrl}/{id}");
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<bool> UpdateInventoryAsync(UpdateInventoryRequest request)
    {
        try
        {
            var response = await http.PutAsJsonAsync($"{BaseUrl}/UpdateInventory", request);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }
}
