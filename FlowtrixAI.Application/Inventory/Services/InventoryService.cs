using FlowtrixAI.Application.Inventory.Interface;
using FlowtrixAI.Domain.Entities;
using FlowtrixAI.Domain.Exceptions;
using FlowtrixAI.Domain.Repositories;

namespace FlowtrixAI.Application.Inventory.Services;

internal class InventoryService(IInventoryRepository _inventoryRepository) 
    : IInventoryService
{
    public async Task AddItemAsync(InventoryItem item)
    {
        item.UpdateAt = DateTime.UtcNow;
        await _inventoryRepository.AddAsync(item);
    }

    /// <summary>
    /// Deducts the specified quantity from the inventory for the given component asynchronously.
    /// </summary>
    /// <param name="componentName">The name of the component from which to deduct inventory. Cannot be null or empty.</param>
    /// <param name="quantity">The amount to deduct from the component's inventory. Must be a non-negative value.</param>
    /// <returns>A task that represents the asynchronous deduction operation.</returns>

    public async Task DeductAsync(string componentName, decimal quantity)
    {
        var items = await _inventoryRepository.GetAllAsync();
        var item = items.FirstOrDefault(x=> x.ComponentName == componentName);

        if (item == null)
            throw new NotFoundException($"Component '{componentName}' not found in inventory.");

        if (item.QuantityAvailable < quantity)
            throw new Exception($"Not enough Quantity for {componentName}");

        item.QuantityAvailable -= quantity;
        item.UpdateAt = DateTime.UtcNow;

        await _inventoryRepository.UpdateAsync(item);


    }

    public async Task<IEnumerable<InventoryItem>> GetAllAsync()=> await _inventoryRepository.GetAllAsync();
   

    public async Task<InventoryItem?> GetByIdAsync(int id)=> await _inventoryRepository.GetByIdAsync(id);


    /// <summary>
    ///  Check availability of a component in inventory based on the required quantity. This method will return true if the available quantity of the specified component is greater than or equal to the required quantity, indicating that the component is available for use. Otherwise, it will return false, indicating that there is insufficient quantity of the component in inventory to meet the requirement.
    /// </summary>
    /// <param name="componentName"></param>
    /// <param name="requiredQuantity"></param>
    /// <returns>True if the component is available in the required quantity, otherwise false.</returns>
    public async Task<bool> IsAvailableAsync(string componentName, decimal requiredQuantity)
    {
        var items = await _inventoryRepository.GetAllAsync();
        var item = items.FirstOrDefault(x=>x.ComponentName == componentName);

        if (item == null) 
            return false;
        
        return item.QuantityAvailable >= requiredQuantity;
    }

    public Task UpdateItemAsync(InventoryItem item)
    {
        item.UpdateAt = DateTime.UtcNow;
        return _inventoryRepository.UpdateAsync(item);
    }
}
