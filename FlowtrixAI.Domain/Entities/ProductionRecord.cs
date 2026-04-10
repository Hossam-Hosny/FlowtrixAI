namespace FlowtrixAI.Domain.Entities;

public class ProductionRecord
{
    public ProductionRecord()
    {
        QualityChecks = new HashSet<QualityCheck>();
    }

    public int Id { get; set; }
    public decimal QuantityProduced { get; set; }
    public DateTime ProduceAd { get; set; }
    public string Notes { get; set; }


    public int ProcessId { get; set; }
    public Process Process { get; set; }

    public int EngineerId { get; set; }
    public AppUser Engineer { get; set; }

    public ICollection<QualityCheck> QualityChecks { get; set; }



}
