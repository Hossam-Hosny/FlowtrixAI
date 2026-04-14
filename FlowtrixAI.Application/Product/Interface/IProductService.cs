using FlowtrixAI.Application.Product.Dtos;
using FlowtrixAI.Domain.Entities;

namespace FlowtrixAI.Application.Product.Interface;

public interface IProductService
{
    Task<bool> CreateProductAsync(CreateProductDto createProductDto, int userId);
    Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync();
    Task<ProductResponseDto?> GetProductByIdAsync(int id);
}
