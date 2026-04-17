namespace FlowtrixAI.Application.Product.Dtos;

public class CreateBOMDto
{
    public string ComponentName { get; set; } = string.Empty;
    public decimal QuantityRequired { get; set; }
    public string Unit { get; set; } = string.Empty;
}
