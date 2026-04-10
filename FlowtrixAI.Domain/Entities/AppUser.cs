namespace FlowtrixAI.Domain.Entities;

public class AppUser
{
    public AppUser()
    {
        ProductionRecords = new HashSet<ProductionRecord>();
        QualityChecks = new HashSet<QualityCheck>();
        Reports = new HashSet<Report>();
    }


    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; } 
    public DateTime CreatedAt { get; set; }

   
    public ICollection<ProductionRecord> ProductionRecords { get; set; }
    public ICollection<QualityCheck> QualityChecks { get; set; }
    public ICollection<Report> Reports { get; set; }

}
