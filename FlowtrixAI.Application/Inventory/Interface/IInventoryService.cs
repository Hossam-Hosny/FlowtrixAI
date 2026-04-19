using FlowtrixAI.Application.Inventory.Dtos;
using FlowtrixAI.Domain.Entities;

namespace FlowtrixAI.Application.Inventory.Interface;

public interface IInventoryService
{
    Task<IEnumerable<InventoryResponseDto>> GetAllAsync();
    Task<InventoryItem?> GetByIdAsync(int id);
    Task<bool> AddItemAsync(CreateInventoryDto dto,int userId);
    Task<bool> UpdateItemAsync (UpdateInventoryDto item,int userId);
    Task <bool> IsAvailableAsync (string componentName,decimal requiredQuantity);
    Task DeductAsync (string componentName, decimal quantity);
    Task<bool> DeleteItemAsync(int id);
}
