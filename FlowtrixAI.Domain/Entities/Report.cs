
namespace FlowtrixAI.Domain.Entities;

public class Report
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Type { get; set; }
    public DateTime GeneratedAt { get; set; }
    public string Data { get; set; }


    public int GeneratedById { get; set; }
    public AppUser GeneratedBy { get; set; }
}
