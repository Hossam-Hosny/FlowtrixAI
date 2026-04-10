namespace FlowtrixAI.Domain.Entities;

public class InventoryItem
{
    public int Id { get; set; }
    public string ComponentName { get; set; }
    public decimal QuantityAvailable { get; set; }
    public string Unit { get; set; }
    public DateTime UpdateAt { get; set; }

    
    public int UpdatedById { get; set; }
    public AppUser UpdatedBy { get; set; }
}
