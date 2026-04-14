using FlowtrixAI.Application.Product.Dtos;
using FlowtrixAI.Application.Product.Interface;
using FlowtrixAI.Domain.Repositories;

namespace FlowtrixAI.Application.Product.Services;

internal class ProductService(IProductRepository _productRepository) : IProductService
{
    public async Task<bool> CreateProductAsync(CreateProductDto createProductDto , int userId)
    {
        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var fileName = Guid.NewGuid() + Path.GetExtension(createProductDto.Image.FileName);
        var fullPath = Path.Combine(folderPath, fileName);

        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await createProductDto.Image.CopyToAsync(stream);
        }


       


        var product = new Domain.Entities.Product
        {
            Name = createProductDto.Name,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userId.ToString(),

            Description = createProductDto.Description,
            ImagePath = $"/images/products/{fileName}",

        };

        await _productRepository.AddAsync(product);
        return (true);

    }

    public async Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();

        var productResponseDtos = products.Select(product => new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            ImagePath = product.ImagePath,
            
            BillOfMaterials = product.BOMs?.Select(bom => new BoMsDto
            {
                ComponentName = bom.ComponentName,
                QuantityRequired = bom.QuantityRequired,
                Unit = bom.Unit
            }).ToList()
        });

        return productResponseDtos;
    }

    public async Task<ProductResponseDto?> GetProductByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        
        return product == null ? null : new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            ImagePath = product.ImagePath,
            BillOfMaterials = product.BOMs?.Select(bom => new BoMsDto
            {
                ComponentName = bom.ComponentName,
                QuantityRequired = bom.QuantityRequired,
                Unit = bom.Unit
            }).ToList()
        };
    }
}