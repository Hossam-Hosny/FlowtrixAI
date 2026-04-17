namespace FlowtrixAi.Client.Models;

public class ProductResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ProductCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImagePath { get; set; } = string.Empty;
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
    public string Name { get; set; } = string.Empty;
    public string ProductCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<CreateBOMItemRequest> BOMs { get; set; } = new();
    public List<CreateProcessStepRequest> ProductionSteps { get; set; } = new();
}

public class CreateBOMItemRequest
{
    public string ComponentName { get; set; } = string.Empty;
    public decimal QuantityRequired { get; set; }
    public string Unit { get; set; } = string.Empty;
}

public class CreateProcessStepRequest
{
    public string StepName { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
    public int Sequence { get; set; } = 1;
}
