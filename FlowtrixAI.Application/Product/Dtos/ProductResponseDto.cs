namespace FlowtrixAI.Application.Product.Dtos;

public class ProductResponseDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? ProductCode { get; set; }
    public string? Description { get; set; }
    public string? ImagePath { get; set; }


    public List<BoMsDto>? BillOfMaterials { get; set; } 
    public List<ProcessResponseDto>? ProductionSteps { get; set; }
}
