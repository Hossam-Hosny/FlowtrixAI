namespace FlowtrixAi.Client.Models;

public class ProductionOrderOperationResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int OrderId { get; set; }
}

public class CreateProductionOrderRequest
{
    public int ProductId { get; set; }
    public int? Quantity { get; set; }
}

public class ProductionOrderResponse
{
    public int OrderId { get; set; }
    public string OrderName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int Progress { get; set; } // Added back
    public DateTime OrderdAt { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? ProblemDescription { get; set; }
    public int? ReportedByUserId { get; set; }
    public string? ReportedByUserName { get; set; }
    public int CurrentStepIndex { get; set; }
}

public class DashboardStats
{
    public int TotalOrders { get; set; }
    public int InProgressCount { get; set; }
    public int CompletedCount { get; set; }
    public int FailedCount { get; set; }
    public int ReadyToStartCount { get; set; }
}
