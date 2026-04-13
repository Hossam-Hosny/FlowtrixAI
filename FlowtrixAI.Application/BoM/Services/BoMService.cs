using FlowtrixAI.Application.BoM.Dtos;
using FlowtrixAI.Application.BoM.Interface;
using FlowtrixAI.Domain.Entities;
using FlowtrixAI.Domain.Repositories;

namespace FlowtrixAI.Application.BoM.Services;

internal class BoMService(IBomRepository _bomRepository,IProductRepository _productRepository) : IBoMService
{
    public async Task<bool> AddBoMForProduct(AddBoMdto dto)
    {
        var product = await _productRepository.GetByIdAsync(dto.ProductId);
        if (product == null) 
            return false;

         var bom = new BillOfMaterial
        {
            ProductId = dto.ProductId,
            ComponentName = dto.ComponentName,
            QuantityRequired = dto.Quantity,
            Unit = dto.Unit.ToString()
            
        };

        await _bomRepository.AddAsync(bom);
        return (true);
    }
}
