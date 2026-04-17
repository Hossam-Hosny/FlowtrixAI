namespace FlowtrixAI.Domain.Entities;

public class ProductionOrder
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public string Status { get; set; } // Pending, Approved, InProgress, Rejected, Completed
    public DateTime CreatedAt { get; set; }
    public int CreatedBy { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }

    public string? ProblemDescription { get; set; }
    public int? ReportedByUserId { get; set; }
    public AppUser? ReportedBy { get; set; }

    public int CurrentStepIndex { get; set; }
}
