namespace FlowtrixAI.Application.Production.Dtos;

public class CreateProductionDto
{
    public int ProcessId { get; set; }
    public int QuantityProduced { get; set; }
    public DateTime Date {  get; set; }
}
