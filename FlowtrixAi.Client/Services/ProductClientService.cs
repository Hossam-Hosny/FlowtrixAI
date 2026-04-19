using System.Net.Http.Json;
using FlowtrixAi.Client.Models;

namespace FlowtrixAi.Client.Services;

public class ProductClientService(HttpClient http)
{
    private const string BaseUrl = "api/Product";

    public async Task<List<ProductResponse>> GetAllProductsAsync()
    {
        try
        {
            var response = await http.GetFromJsonAsync<List<ProductResponse>>($"{BaseUrl}/GetAllProducts");
            return response ?? new List<ProductResponse>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching products: {ex.Message}");
            return new List<ProductResponse>();
        }
    }

    public async Task<ProductResponse?> GetProductByIdAsync(int id)
    {
        try
        {
            return await http.GetFromJsonAsync<ProductResponse>($"{BaseUrl}/GetProductById/{id}");
        }
        catch { return null; }
    }

    public async Task<bool> CreateProductAsync(CreateProductRequest request)
    {
        try
        {
            var response = await http.PostAsJsonAsync($"{BaseUrl}/CreateProduct", request);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<bool> UpdateProductAsync(int id, CreateProductRequest request)
    {
        try
        {
            var response = await http.PutAsJsonAsync($"{BaseUrl}/UpdateProduct/{id}", request);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        try
        {
            var response = await http.DeleteAsync($"{BaseUrl}/DeleteProduct/{id}");
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }
}
