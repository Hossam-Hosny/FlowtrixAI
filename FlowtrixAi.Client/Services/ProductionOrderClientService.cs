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
        
        return new DashboardStats
        {
            TotalOrders = orders.Count,
            ReadyToStartCount = orders.Count(o => o.Status?.ToLower() == "approved"),
            InProgressCount = orders.Count(o => o.Status?.ToLower() == "started" || o.Status?.ToLower() == "inprogress" || o.Status?.ToLower() == "in progress" || o.Status?.ToLower() == "processing"),
            CompletedCount = orders.Count(o => o.Status?.ToLower() == "completed"),
            FailedCount = orders.Count(o => o.Status?.ToLower() == "failed" || o.Status?.ToLower() == "rejected")
        };
    }
    public async Task<ProductionOrderOperationResponse> CreateOrderAsync(CreateProductionOrderRequest request)
    {
        try
        {
            var response = await http.PostAsJsonAsync($"{BaseUrl}/create", request);
            return await response.Content.ReadFromJsonAsync<ProductionOrderOperationResponse>() 
                ?? new ProductionOrderOperationResponse { Success = false, Message = "فشل في فك تشفير البيانات" };
        }
        catch (Exception ex)
        {
            return new ProductionOrderOperationResponse { Success = false, Message = ex.Message };
        }
    }

    public async Task<string> StartOrderAsync(int orderId)
    {
        try
        {
            var response = await http.PostAsync($"{BaseUrl}/{orderId}/start", null);
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }

    public async Task<string> CompleteOrderAsync(int orderId)
    {
        try
        {
            var response = await http.PostAsync($"{BaseUrl}/{orderId}/complete", null);
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }

    public async Task<string> FailOrderAsync(int orderId, string problemDescription)
    {
        try
        {
            var response = await http.PostAsJsonAsync($"{BaseUrl}/{orderId}/fail", new { ProblemDescription = problemDescription });
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }
    public async Task<string> UpdateOrderProgressAsync(int orderId, int stepIndex)
    {
        try
        {
            var response = await http.PostAsync($"{BaseUrl}/{orderId}/progress?stepIndex={stepIndex}", null);
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }
    public async Task<ProductionOrderResponse?> GetOrderByIdAsync(int id)
    {
        try
        {
            return await http.GetFromJsonAsync<ProductionOrderResponse>($"{BaseUrl}/{id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching order {id}: {ex.Message}");
            return null;
        }
    }
    public async Task<string> CancelOrderAsync(int orderId)
    {
        try
        {
            var response = await http.DeleteAsync($"{BaseUrl}/{orderId}/cancel");
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }
}
