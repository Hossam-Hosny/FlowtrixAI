using Microsoft.AspNetCore.Http;

namespace FlowtrixAI.Application.Product.Dtos;

public class CreateProductDto
{
    public string Name { get; set; } = string.Empty;
    public string ProductCode { get; set; } = string.Empty;
    public string? ImagePath { get; set; }
    public List<BoMsDto>? BillOfMaterials { get; set; }
    public List<ProcessResponseDto>? ProductionSteps { get; set; }
    public string Description { get; set; } = string.Empty;
    public List<CreateBOMDto> BOMs { get; set; } = new();
}
