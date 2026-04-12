namespace FlowtrixAI.Application.Quality.Dtos;

public class QualityResponseDto
{
    public int Id { get; set; }
    public int ProductionRecordId { get; set; }
    public bool IsPassed { get; set; }
    public int DefectCount { get; set; } 
    public string Notes { get; set; }
    public int CheckedById { get; set; }


}
