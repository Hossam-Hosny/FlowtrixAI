namespace FlowtrixAI.Domain.Entities;

public class QualityCheck
{
    public int Id { get; set; }
    public int DecfectCount { get; set; } 
    public DateTime CheckAt { get; set; }
    public string Notes { get; set; }

    public bool IsPassed => DecfectCount == 0;

    public int ProductionRecordId { get; set; }
    public ProductionRecord ProductionRecord { get; set; }

    public int CheckedById { get; set; }
    public AppUser CheckedBy { get; set; }
}
