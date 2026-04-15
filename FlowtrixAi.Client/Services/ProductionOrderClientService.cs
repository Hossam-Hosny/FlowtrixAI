using System.Net.Http.Json;
using FlowtrixAi.Client.Models;

namespace FlowtrixAi.Client.Services;

public class ProductionOrderClientService(HttpClient http)
{
    private const string BaseUrl = "api/ProductionOrder";

    public async Task<List<ProductionOrderResponse>> GetAllOrdersAsync()
    {
        try
        {
            var response = await http.GetFromJsonAsync<List<ProductionOrderResponse>>($"{BaseUrl}/GetAll");
            return response ?? new List<ProductionOrderResponse>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching orders: {ex.Message}");
            return new List<ProductionOrderResponse>();
        }
    }

    public async Task<int> GetCompletedOrdersCountAsync()
    {
        try
        {
            var response = await http.GetAsync($"{BaseUrl}/CompletedOrdersNo");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return int.TryParse(content, out var count) ? count : 0;
            }
            return 0;
        }
        catch { return 0; }
    }

    public async Task<int> GetFailedOrdersCountAsync()
    {
        try
        {
            var response = await http.GetAsync($"{BaseUrl}/FaildOrdersNo");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return int.TryParse(content, out var count) ? count : 0;
            }
            return 0;
        }
        catch { return 0; }
    }

    public async Task<DashboardStats> GetDashboardStatsAsync()
    {
        var orders = await GetAllOrdersAsync();
        var completed = await GetCompletedOrdersCountAsync();
        var failed = await GetFailedOrdersCountAsync();
        
        return new DashboardStats
        {
            TotalOrders = orders.Count,
            InProgressCount = orders.Count(o => o.Status?.ToLower() == "started" || o.Status?.ToLower() == "in progress" || o.Status?.ToLower() == "processing"),
            CompletedCount = completed,
            FailedCount = failed
        };
    }
}
