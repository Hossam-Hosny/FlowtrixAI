namespace FlowtrixAI.Domain.Entities;

public class BillOfMaterial
{
    public int Id { get; set; }
    public string ComponentName { get; set; }
    public decimal QuantityRequired { get; set; }
    public string Unit { get; set; }


    public int ProductId { get; set; }
    public Product Product { get; set; }
}
