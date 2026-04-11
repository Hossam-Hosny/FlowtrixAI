using FlowtrixAI.Domain.Entities;

namespace FlowtrixAI.Application.Inventory.Interface;

public interface IInventoryService
{
    Task<IEnumerable<InventoryItem>> GetAllAsync();
    Task<InventoryItem?> GetByIdAsync(int id);
    Task AddItemAsync(InventoryItem item);
    Task UpdateItemAsync (InventoryItem item);
    Task <bool> IsAvailableAsync (string componentName,decimal requiredQuantity);
    Task DeductAsync (string componentName, decimal quantity);
}
