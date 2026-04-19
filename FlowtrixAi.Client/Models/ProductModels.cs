using System.ComponentModel.DataAnnotations;

namespace FlowtrixAi.Client.Models;

public class ProductResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ProductCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImagePath { get; set; } = string.Empty;
    public decimal StockQuantity { get; set; }
    public List<BOMItemResponse> BillOfMaterials { get; set; } = new();
    public List<ProcessStepResponse> ProductionSteps { get; set; } = new();
}

public class BOMItemResponse
{
    public string ComponentName { get; set; } = string.Empty;
    public decimal QuantityRequired { get; set; }
    public string Unit { get; set; } = string.Empty;
}

public class ProcessStepResponse
{
    public int Id { get; set; }
    public string StepName { get; set; } = string.Empty;
    public int Sequence { get; set; }
    public int DurationMinutes { get; set; }
}

public class CreateProductRequest
{
    [Required(ErrorMessage = "اسم المنتج مطلوب")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "كود المنتج مطلوب")]
    public string ProductCode { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
    public List<CreateBOMItemRequest> BOMs { get; set; } = new();
    public List<CreateProcessStepRequest> ProductionSteps { get; set; } = new();
}

public class CreateBOMItemRequest
{
    [Required(ErrorMessage = "المكون مطلوب")]
    public string ComponentName { get; set; } = string.Empty;

    [Required(ErrorMessage = "الكمية مطلوبة")]
    [Range(0.001, double.MaxValue, ErrorMessage = "الكمية يجب أن تكون أكبر من صفر")]
    public decimal? QuantityRequired { get; set; }

    [Required(ErrorMessage = "الوحدة مطلوبة")]
    public string Unit { get; set; } = string.Empty;
}

public class CreateProcessStepRequest
{
    [Required(ErrorMessage = "اسم الخطوة مطلوب")]
    public string StepName { get; set; } = string.Empty;

    [Required(ErrorMessage = "المدة مطلوبة")]
    [Range(1, int.MaxValue, ErrorMessage = "يجب أن تكون المدة أكبر من صفر")]
    public int? DurationMinutes { get; set; }
    public int Sequence { get; set; } = 1;
}
