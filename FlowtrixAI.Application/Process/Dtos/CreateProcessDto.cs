namespace FlowtrixAI.Application.Process.Dtos;

public class CreateProcessDto
{
    public int ProductId { get; set; }
    public string StepName { get; set; }
    public int EstimatedTimeInHours { get; set; }
}
