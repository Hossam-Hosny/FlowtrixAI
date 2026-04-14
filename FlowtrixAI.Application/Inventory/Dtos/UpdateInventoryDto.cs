namespace FlowtrixAI.Application.Inventory.Dtos;

public class UpdateInventoryDto
{
    public int ComponentId { get; set; }
    public string MaterialName { get; set; }
    public int Quantity { get; set; }
}
