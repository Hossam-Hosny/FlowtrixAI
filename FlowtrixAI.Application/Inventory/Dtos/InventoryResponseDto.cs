namespace FlowtrixAI.Application.Inventory.Dtos;

public class InventoryResponseDto
{
    public int ComponentId { get; set; }
    public string ComponentName { get; set; }
    public decimal QuantityAvailable { get; set; }
    public string Unit { get; set; }
    public DateTime UpdateAt { get; set; }


    public int UpdatedById { get; set; }
}
