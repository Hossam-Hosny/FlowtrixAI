namespace FlowtrixAI.Application.Reports.Dtos;

public class ReportDto
{
    public int TotalOrders { get; set; }
    public int ApprovedOrders { get; set; }
    public int CompletedOrders { get; set; }
    public int FaildOrders { get; set; }
    public int DeleverdOrders { get; set; }
    public int TotalProduction => ApprovedOrders+CompletedOrders+DeleverdOrders;
    public int TotalInventoryItems { get; set; }
}
