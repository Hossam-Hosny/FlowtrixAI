namespace FlowtrixAI.Application.ProductionOrder.Dtos;

public class ProductionOrdersResponse
{
    public int OrderId { get; set; }
    public string OrderName { get; set; }
    public decimal Quantity { get; set; }
    public DateTime OrderdAt { get; set; }
    public string Status { get; set; }
}
