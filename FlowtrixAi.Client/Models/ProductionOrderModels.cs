namespace FlowtrixAi.Client.Models;

public class ProductionOrderResponse
{
    public int OrderId { get; set; }
    public string OrderName { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public DateTime OrderdAt { get; set; }
    public string Status { get; set; } = string.Empty;
    
    // For UI progress calculation
    public int Progress => Status?.ToLower() switch
    {
        "completed" => 100,
        "failed" => 0,
        "started" or "in progress" or "processing" => 60, // Default for now
        "pending" => 0,
        _ => 0
    };
}

public class DashboardStats
{
    public int TotalOrders { get; set; }
    public int InProgressCount { get; set; }
    public int CompletedCount { get; set; }
    public int FailedCount { get; set; }
}
