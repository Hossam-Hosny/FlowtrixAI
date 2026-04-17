namespace FlowtrixAI.Application.Export.Dtos
{
    public class CreateExportDto
    {
        public string OrderNumber { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public string BuyerName { get; set; } = string.Empty;
        public string DestinationCountry { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
    }

    public class ExportDto
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public string BuyerName { get; set; } = string.Empty;
        public string DestinationCountry { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
