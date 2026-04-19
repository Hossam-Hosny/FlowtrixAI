using System.Net.Http.Json;
using FlowtrixAi.Client.Models;

namespace FlowtrixAi.Client.Services
{
    public class MaintenanceClientService(HttpClient _http)
    {
        public async Task<ResponseModel> ResetDatabaseAsync(string password)
        {
            var response = await _http.PostAsJsonAsync("api/Maintenance/reset-database", new { Password = password });
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ResponseModel>() ?? new ResponseModel { Message = "تمت العملية بنجاح" };
            }
            
            var error = await response.Content.ReadFromJsonAsync<ResponseModel>();
            return error ?? new ResponseModel { Message = "فشل في تنفيذ العملية" };
        }
    }

    public class ResponseModel
    {
        public string Message { get; set; } = string.Empty;
        public string? Error { get; set; }
    }
}
