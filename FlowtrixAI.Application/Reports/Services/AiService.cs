using FlowtrixAI.Application.Reports.Dtos;
using FlowtrixAI.Application.Reports.Interface;
using FlowtrixAI.Domain.Constants;
using FlowtrixAI.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace FlowtrixAI.Application.Reports.Services;

internal class AiService(
    IProductionOrderRepository _productionOrderRepository,
    IInventoryRepository _inventoryRepository,
    IBomRepository _bomRepository,
    IProductRepository _productRepository,
    IExportRepository _exportRepository,
    IProductionRecordRepository _productionRecordRepository,
    IQualityCheckRepository _qualityCheckRepository,
    IAiChatRepository _aiChatRepository,
    IConfiguration _configuration,
    IHttpClientFactory _httpClientFactory) : IAiService
{
    private List<string> GeminiApiKeys => _configuration.GetSection("Gemini:ApiKeys").Get<List<string>>() ?? new List<string>();

    public async Task<AiInsightDto> GenerateInsightsAsync()
    {
        var orders = await _productionOrderRepository.GetAllAsync();
        var inventory = await _inventoryRepository.GetAllAsync();

        var insights = new List<string>();

        // low Inventory Detection
        foreach (var item in inventory)
        {
            if (item.QuantityAvailable < item.MinimumStockLevel)
                insights.Add($"⚠️ Low stock alert: {item.ComponentName} is below safe level ({item.QuantityAvailable}/{item.MinimumStockLevel})");
        }

        // Top Product Insight
        var topProduct = orders.GroupBy(o => o.Product?.Name ?? "Unknown")
                                   .Select(g => new
                                   {
                                       Name = g.Key,
                                       Total = g.Sum(x => x.Quantity)
                                   })
                                   .OrderByDescending(x => x.Total)
                                   .FirstOrDefault();

        if (topProduct != null)
            insights.Add($"🔥 Top product: {topProduct.Name} with demand of {topProduct.Total}");

        // Failure Rate
        var failed = orders.Count(o => o.Status == OrderSteps.Rejected);
        var total = orders.Count();

        if (total > 0)
        {
            var failureRate = (failed * 100) / total;
            if (failureRate > 30)
                insights.Add($"❌ High failure rate detected: {failureRate}% of orders failed");
        }

        return new AiInsightDto
        {
            AiInsightIds = insights
        };
    }

    public async Task<string> GenerateFullReportAsync()
    {
        var context = await GetSystemStateAsTextAsync();
        var prompt = $"أنت خبير في إدارة المصانع والعمليات. بناءً على البيانات التالية المستخرجة من نظام FlowtrixAI، قم بإنشاء تقرير مفصل واحترافي باللغة العربية.\n" +
                     $"يجب أن يتضمن التقرير:\n" +
                     $"1. ملخص تنفيذي لحالة المنظمة.\n" +
                     $"2. تحليل للمنتجات والأكثر طلباً.\n" +
                     $"3. حالة طلبات الإنتاج والمشاكل الحالية.\n" +
                     $"4. وضع المخازن والمواد الخام ونقص المخزون.\n" +
                     $"5. تحليل لعمليات التصدير (Exports).\n" +
                     $"6. توصيات استراتيجية لتحسين الكفاءة واتخاذ القرار.\n\n" +
                     $"البيانات:\n{context}";

        return await CallGeminiAsync(prompt);
    }

    public async Task<string> ChatWithAiAsync(string userId, string chatId, List<FlowtrixAI.Application.Reports.Dtos.AiChatMessageDto> history, string userMessage)
    {
        var context = await GetSystemStateAsTextAsync();
        var systemPrompt = $"أنت مساعد ذكي لنظام إدارة المصانع FlowtrixAI. إليك الحالة الحالية للنظام:\n{context}\n\n" +
                           "يرجى تقديم ردود دقيقة ومفيدة بناءً على سياق المحادثة والبيانات المتاحة.";

        var contents = new List<object>();
        contents.Add(new { role = "user", parts = new[] { new { text = systemPrompt } } });
        contents.Add(new { role = "model", parts = new[] { new { text = "فهمت تماماً. أنا الآن مطلع على حالة نظام FlowtrixAI وجاهز للإجابة على استفساراتك بناءً على هذه البيانات." } } });

        if (history != null)
        {
            foreach (var h in history)
            {
                contents.Add(new { role = h.Role, parts = new[] { new { text = h.Text } } });
            }
        }

        contents.Add(new { role = "user", parts = new[] { new { text = userMessage } } });

        var response = await CallGeminiRawAsync(new { contents = contents });

        await _aiChatRepository.AddHistoryAsync(new FlowtrixAI.Domain.Entities.AiChatHistory
        {
            UserId = userId,
            ChatId = chatId, // حفظ الـ ChatId لربط الرسائل ببعضها
            UserMessage = userMessage,
            AiResponse = response,
            InteractionType = "Chat",
            Timestamp = DateTime.UtcNow
        });

        return response;
    }

    public async Task<IEnumerable<FlowtrixAI.Domain.Entities.AiChatHistory>> GetUserChatSessionsAsync(string userId)
    {
        return await _aiChatRepository.GetUserChatSessionsAsync(userId);
    }

    public async Task<IEnumerable<FlowtrixAI.Domain.Entities.AiChatHistory>> GetChatMessagesBySessionAsync(string chatId)
    {
        return await _aiChatRepository.GetMessagesByChatIdAsync(chatId);
    }

    public async Task DeleteChatSessionAsync(string chatId)
    {
        await _aiChatRepository.DeleteChatSessionAsync(chatId);
    }

    private async Task<string> GetSystemStateAsTextAsync()
    {
        var products = await _productRepository.GetTopAsync(30);
        var orders = await _productionOrderRepository.GetLatestAsync(20);
        var inventory = await _inventoryRepository.GetTopAsync(40);
        var exports = await _exportRepository.GetLatestAsync(20);
        var productionRecords = await _productionRecordRepository.GetLatestAsync(15);
        var qualityChecks = await _qualityCheckRepository.GetLatestAsync(15);

        var data = new
        {
            Products = products.Select(p => new { p.Name, p.ProductCode, p.StockQuantity }),
            RecentProductionOrders = orders.Select(o => new { o.Id, ProductName = o.Product?.Name, o.Quantity, o.Status, o.CreatedAt }),
            Inventory = inventory.Select(i => new { i.ComponentName, i.QuantityAvailable, i.MinimumStockLevel, i.Unit }),
            RecentExports = exports.Select(e => new { e.BuyerName, e.DestinationCountry, e.Status, e.Quantity, e.TotalAmount }),
            RecentProductionHistory = productionRecords.Select(r => new { r.QuantityProduced, r.Notes, r.ProduceAd }),
            RecentQualityStatus = qualityChecks.Select(q => new { q.IsPassed, q.DecfectCount, q.Notes, q.CheckAt })
        };

        return JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
    }

    private async Task<string> CallGeminiAsync(string prompt)
    {
        var requestBody = new
        {
            contents = new[]
            {
                new { parts = new[] { new { text = prompt } } }
            }
        };
        return await CallGeminiRawAsync(requestBody);
    }

    private async Task<string> CallGeminiRawAsync(object requestBody)
    {
        var keys = GeminiApiKeys.Where(k => !string.IsNullOrEmpty(k)).ToList();

        if (!keys.Any())
            return "❌ خطأ: لم يتم العثور على مفاتيح API في الإعدادات.";

        var client = _httpClientFactory.CreateClient();
        var jsonRequest = JsonSerializer.Serialize(requestBody);
        var errors = new List<string>();

        foreach (var key in keys)
        {
            // نستخدم Gemini 1.5 Flash أو الموديل المحدد في الكود الأصلي
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={key}";
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(url, content);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(responseBody);
                    var text = doc.RootElement
                        .GetProperty("candidates")[0]
                        .GetProperty("content")
                        .GetProperty("parts")[0]
                        .GetProperty("text")
                        .GetString();

                    return text ?? "⚠️ الـ AI لم يعطِ ردّاً.";
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                errors.Add($"Key ({key.Substring(0, Math.Min(5, key.Length))}...): {response.StatusCode}");
            }
            catch (Exception ex)
            {
                errors.Add($"Key Error: {ex.Message}");
            }
        }

        return $"❌ فشلت جميع المحاولات باستخدام {keys.Count} مفاتيح. الأخطاء: {string.Join(" | ", errors)}";
    }
}
