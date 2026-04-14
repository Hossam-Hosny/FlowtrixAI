namespace FlowtrixAI.Application.Production.Dtos;

public class ProductionResponseDto
{
    public int Id { get; set; }
    public int ProcessId { get; set; }
    public int QuantityProduced { get; set; }
    public DateTime Date { get; set; } 
    public int EngineerId { get; set; }
}
