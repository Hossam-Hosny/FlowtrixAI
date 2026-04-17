using FlowtrixAI.Application.Product.Dtos;
using FlowtrixAI.Application.Product.Interface;
using FlowtrixAI.Domain.Repositories;

namespace FlowtrixAI.Application.Product.Services;

internal class ProductService(IProductRepository _productRepository) : IProductService
{
    public async Task<bool> CreateProductAsync(CreateProductDto createProductDto , int userId)
    {
        var product = new Domain.Entities.Product
        {
            Name = createProductDto.Name,
            ProductCode = createProductDto.ProductCode,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userId.ToString(),
            Description = createProductDto.Description,
            ImagePath = "",
        };

        if (createProductDto.BOMs != null && createProductDto.BOMs.Any())
        {
            foreach (var bomDto in createProductDto.BOMs)
            {
                product.BOMs.Add(new Domain.Entities.BillOfMaterial
                {
                    ComponentName = bomDto.ComponentName,
                    QuantityRequired = bomDto.QuantityRequired,
                    Unit = bomDto.Unit
                });
            }
        }

        if (createProductDto.ProductionSteps != null && createProductDto.ProductionSteps.Any())
        {
            foreach (var stepDto in createProductDto.ProductionSteps)
            {
                product.Processes.Add(new Domain.Entities.Process
                {
                    StepName = stepDto.StepName,
                    Sequence = stepDto.Sequence,
                    DurationMinutes = stepDto.DurationMinutes,
                    Resource = "" // Default
                });
            }
        }

        await _productRepository.AddAsync(product);
        return true;

    }

    public async Task<bool> UpdateProductAsync(int id, CreateProductDto updateProductDto, int userId)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null) return false;

        product.Name = updateProductDto.Name;
        product.ProductCode = updateProductDto.ProductCode;
        product.Description = updateProductDto.Description;
        product.UpdatedAt = DateTime.UtcNow;
        product.UpdatedBy = userId.ToString();

        // Update BOMs - Simplest way is to clear and re-add
        product.BOMs.Clear();
        product.Processes.Clear();

        if (updateProductDto.BOMs != null)
        {
            foreach (var bomDto in updateProductDto.BOMs)
            {
                product.BOMs.Add(new Domain.Entities.BillOfMaterial
                {
                    ComponentName = bomDto.ComponentName,
                    QuantityRequired = bomDto.QuantityRequired,
                    Unit = bomDto.Unit
                });
            }
        }

        if (updateProductDto.ProductionSteps != null)
        {
            foreach (var stepDto in updateProductDto.ProductionSteps)
            {
                product.Processes.Add(new Domain.Entities.Process
                {
                    StepName = stepDto.StepName,
                    Sequence = stepDto.Sequence,
                    DurationMinutes = stepDto.DurationMinutes,
                    Resource = ""
                });
            }
        }

        await _productRepository.UpdateAsync(product);
        return true;
    }

    public async Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();

        var productResponseDtos = products.Select(product => new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            ProductCode = product.ProductCode,
            Description = product.Description,
            ImagePath = product.ImagePath,
            StockQuantity = product.StockQuantity,
            
            BillOfMaterials = product.BOMs?.Select(bom => new BoMsDto
            {
                ComponentName = bom.ComponentName,
                QuantityRequired = bom.QuantityRequired,
                Unit = bom.Unit
            }).ToList(),
            ProductionSteps = product.Processes?.OrderBy(p => p.Sequence).Select(p => new ProcessResponseDto
            {
                Id = p.Id,
                StepName = p.StepName,
                Sequence = p.Sequence,
                DurationMinutes = p.DurationMinutes
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
            ProductCode = product.ProductCode,
            Description = product.Description,
            ImagePath = product.ImagePath,
            StockQuantity = product.StockQuantity,
            BillOfMaterials = product.BOMs?.Select(bom => new BoMsDto
            {
                ComponentName = bom.ComponentName,
                QuantityRequired = bom.QuantityRequired,
                Unit = bom.Unit
            }).ToList(),
            ProductionSteps = product.Processes?.OrderBy(p => p.Sequence).Select(p => new ProcessResponseDto
            {
                Id = p.Id,
                StepName = p.StepName,
                Sequence = p.Sequence,
                DurationMinutes = p.DurationMinutes
            }).ToList()
        };
    }
}