namespace FlowtrixAi.Client.Models;

public class InventoryResponse
{
    public int ComponentId { get; set; }
    public string ComponentName { get; set; } = string.Empty;
    public string MaterialCode { get; set; } = string.Empty;
    public decimal QuantityAvailable { get; set; }
    public decimal TotalIncoming { get; set; }
    public decimal TotalUsed { get; set; }
    public decimal MinimumStockLevel { get; set; }
    public string Unit { get; set; } = string.Empty;
    public DateTime UpdateAt { get; set; }
    
    public bool IsLowStock => QuantityAvailable <= MinimumStockLevel;
}

public class CreateInventoryRequest
{
    public string MaterialName { get; set; } = string.Empty;
    public string MaterialCode { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal MinimumStockLevel { get; set; }
    public string Unit { get; set; } = string.Empty;
}

public class WarehouseStats
{
    public int TotalItemsCount { get; set; }
    public decimal TotalIncoming { get; set; }
    public decimal TotalUsed { get; set; }
    public decimal TotalRemaining { get; set; }
    public int LowStockCount { get; set; }
}
