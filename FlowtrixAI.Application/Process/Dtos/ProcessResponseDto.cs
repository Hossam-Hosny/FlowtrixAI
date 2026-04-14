namespace FlowtrixAI.Application.Process.Dtos;

public class ProcessResponseDto
{
    public int Id { get; set; }
    public string StepName { get; set; }
    public int EstimatedTimeInHours { get; set; }
}
