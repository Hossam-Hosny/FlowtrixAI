namespace FlowtrixAI.Domain.Entities;

public class InventoryItem
{
    public int Id { get; set; }
    public string ComponentName { get; set; }
    public string MaterialCode { get; set; }
    public decimal QuantityAvailable { get; set; }
    public decimal TotalIncoming { get; set; }
    public decimal TotalUsed { get; set; }
    public decimal MinimumStockLevel { get; set; }
    public string Unit { get; set; }
    public DateTime UpdateAt { get; set; }

    
    public int UpdatedById { get; set; }
    public AppUser UpdatedBy { get; set; }
}
