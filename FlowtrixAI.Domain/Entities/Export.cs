namespace FlowtrixAI.Domain.Entities
{
    public class Export
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount => Quantity * UnitPrice;
        public string BuyerName { get; set; } = string.Empty;
        public string DestinationCountry { get; set; } = string.Empty;
        public ExportStatus Status { get; set; } = ExportStatus.Pending;
    }

    public enum ExportStatus
    {
        Pending,   // قيد الانتظار
        Shipped,   // تم الشحن
        Delivered  // تم التوصيل
    }
}
