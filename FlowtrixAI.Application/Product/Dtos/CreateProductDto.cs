using Microsoft.AspNetCore.Http;

namespace FlowtrixAI.Application.Product.Dtos;

public class CreateProductDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public IFormFile Image { get; set; }
}
