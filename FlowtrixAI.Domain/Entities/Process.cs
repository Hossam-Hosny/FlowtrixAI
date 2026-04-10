namespace FlowtrixAI.Domain.Entities;

public class Process
{
    public Process()
    {
        ProductionRecords = new HashSet<ProductionRecord>();
    }

    public int Id { get; set; }
    public string StepName { get; set; }
    public int Sequence { get; set; }
    public int DurationMinutes { get; set; }
    public string Resource {  get; set; }



    public int ProductId { get; set; }
    public Product Product { get; set; }

    public ICollection<ProductionRecord> ProductionRecords { get; set; }




}
