using FlowtrixAI.Application.Reports.Dtos;
using FlowtrixAI.Application.Reports.Interface;
using FlowtrixAI.Domain.Constants;
using FlowtrixAI.Domain.Repositories;

namespace FlowtrixAI.Application.Reports.Services;

internal class AiService(IProductionOrderRepository _productionOrderRepository
    ,IInventoryRepository _inventoryRepository , IBomRepository _bomRepository) : IAiService
{
    public async Task<AiInsightDto> GenerateInsightsAsync()
    {
        var orders= await _productionOrderRepository.GetAllAsync();
        var inventory= await _inventoryRepository.GetAllAsync();

        var insights = new List<string>();

        // low Inventory Detection
        foreach (var item in inventory)
        {
            insights.Add($"⚠️ Low stock alert: {item.ComponentName} is below safe level");
        }

        // Top Product Insight
        var topProduct = orders.GroupBy(o => o.Product.Name)
                                   .Select(g => new
                                   {
                                       Name = g.Key,
                                       Total = g.Sum(x => x.Quantity)
                                   })
                                   .OrderByDescending(x=>x.Total)
                                   .FirstOrDefault();

        if (topProduct != null)
            insights.Add($"🔥 Top product: {topProduct.Name} with demand of {topProduct.Total}");

        // Failure Rate
        var failed = orders.Count(o => o.Status == OrderSteps.Rejected);
        var total = orders.Count();

        if (total > 0)
        {
            var failureRate = (failed * 100 )/total;
            if (failureRate > 30)
                insights.Add($"❌ High failure rate detected: {failureRate}% of orders failed");
            
        }

        // Material Consumption Insight
        var materialUsage = new Dictionary<string, int>();

        foreach (var order in orders)
        {
            var bomItems = await _bomRepository.GetByIdAsync(order.ProductId);
            foreach (var item in bomItems)
            {
                var used = item.QuantityRequired * order.Quantity;
                if (materialUsage.ContainsKey(item.ComponentName))
                    materialUsage[item.ComponentName] += (int)used;
                else
                    materialUsage[item.ComponentName] = (int)used;

            }
        }

        var mostUsed = materialUsage
           .OrderByDescending(x => x.Value)
           .FirstOrDefault();

        if (!string.IsNullOrEmpty(mostUsed.Key))
        {
            insights.Add($"📦 Most consumed material: {mostUsed.Key}");
        }

        return new AiInsightDto
        {
            AiInsightIds = insights
        };

    }
}
