using FlowtrixAI.Application.Product.Dtos;
using FlowtrixAI.Domain.Entities;

namespace FlowtrixAI.Application.Product.Interface;

public interface IProductService
{
    Task<bool> CreateProductAsync(CreateProductDto createProductDto, int userId);
    Task<bool> UpdateProductAsync(int id, CreateProductDto updateProductDto, int userId);
    Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync();
    Task<ProductResponseDto?> GetProductByIdAsync(int id);
    Task<bool> DeleteProductAsync(int id);
}
