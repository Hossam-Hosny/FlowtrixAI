namespace FlowtrixAI.Application.Inventory.Dtos;

public class UpdateInventoryDto
{
    public int ComponentId { get; set; }
    public string MaterialName { get; set; }
    public string MaterialCode { get; set; }
    public decimal Quantity { get; set; }
    public decimal MinimumStockLevel { get; set; }
    public string Unit { get; set; }
}
