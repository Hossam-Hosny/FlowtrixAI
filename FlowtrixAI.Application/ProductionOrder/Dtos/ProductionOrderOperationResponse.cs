namespace FlowtrixAI.Application.ProductionOrder.Dtos;

public class ProductionOrderOperationResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int OrderId { get; set; }
}
