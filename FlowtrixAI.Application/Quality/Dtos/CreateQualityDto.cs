namespace FlowtrixAI.Application.Quality.Dtos;

public class CreateQualityDto
{
    public int ProductionRecordId { get; set; }
    
    public int DefectsCount { get; set; }
    public string Notes { get; set; }
}
