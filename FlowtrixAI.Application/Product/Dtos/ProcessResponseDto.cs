namespace FlowtrixAI.Application.Product.Dtos;

public class ProcessResponseDto
{
    public int Id { get; set; }
    public string StepName { get; set; } = string.Empty;
    public int Sequence { get; set; }
    public int DurationMinutes { get; set; }
}
