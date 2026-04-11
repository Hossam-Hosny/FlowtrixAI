
using Microsoft.AspNetCore.Identity;

namespace FlowtrixAI.Domain.Entities;

public class AppUser : IdentityUser<int>
{
    public AppUser()
    {
        ProductionRecords = new HashSet<ProductionRecord>();
        QualityChecks = new HashSet<QualityCheck>();
        Reports = new HashSet<Report>();
    }

    public string Name { get; set; }
   
    public DateTime CreatedAt { get; set; }

   
    public ICollection<ProductionRecord> ProductionRecords { get; set; }
    public ICollection<QualityCheck> QualityChecks { get; set; }
    public ICollection<Report> Reports { get; set; }

}
