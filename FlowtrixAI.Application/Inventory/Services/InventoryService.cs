using FlowtrixAI.Application.Inventory.Dtos;
using FlowtrixAI.Application.Inventory.Interface;
using FlowtrixAI.Domain.Entities;
using FlowtrixAI.Domain.Exceptions;
using FlowtrixAI.Domain.Repositories;
using System.Security.Claims;

namespace FlowtrixAI.Application.Inventory.Services;

internal class InventoryService(IInventoryRepository _inventoryRepository) 
    : IInventoryService
{
    public async Task<bool> AddItemAsync(CreateInventoryDto _inventoryDto, int userId)
    {
        _inventoryDto.MaterialName = _inventoryDto.MaterialName.Trim().ToLower();
        var component = await _inventoryRepository.GetByNameAsync(_inventoryDto.MaterialName);

        if (component != null)
        {
            component.QuantityAvailable += _inventoryDto.Quantity;
            component.TotalIncoming += _inventoryDto.Quantity;
            component.MinimumStockLevel = _inventoryDto.MinimumStockLevel;
            component.MaterialCode = _inventoryDto.MaterialCode;
            component.UpdatedById = userId;
            component.UpdateAt = DateTime.UtcNow;
            await _inventoryRepository.UpdateAsync(component);
            return true;
        }
        else
        {
            var item = new InventoryItem
            {
                ComponentName = _inventoryDto.MaterialName,
                MaterialCode = _inventoryDto.MaterialCode,
                QuantityAvailable = _inventoryDto.Quantity,
                TotalIncoming = _inventoryDto.Quantity,
                TotalUsed = 0,
                MinimumStockLevel = _inventoryDto.MinimumStockLevel,
                Unit = _inventoryDto.Unit,
                UpdatedById = userId,
                UpdateAt = DateTime.UtcNow,
            };

            await _inventoryRepository.AddAsync(item);
            return true;
        }

      
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
        item.TotalUsed += quantity;
        item.UpdateAt = DateTime.UtcNow;

        await _inventoryRepository.UpdateAsync(item);


    }

    public async Task<IEnumerable<InventoryResponseDto>> GetAllAsync()
    {
        var items = await _inventoryRepository.GetAllAsync();
        return items.Select(x => new InventoryResponseDto
        {
            ComponentId = x.Id,
            ComponentName = x.ComponentName,
            MaterialCode = x.MaterialCode,
            QuantityAvailable = x.QuantityAvailable,
            TotalIncoming = x.TotalIncoming,
            TotalUsed = x.TotalUsed,
            MinimumStockLevel = x.MinimumStockLevel,
            Unit = x.Unit,
            UpdateAt = x.UpdateAt,
            UpdatedById = x.UpdatedById
            
        });
    }
   

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

    public async Task<bool> UpdateItemAsync(UpdateInventoryDto _inventoryDto , int userId)
    {
        var item = await _inventoryRepository.GetByIdAsync(_inventoryDto.ComponentId);

        // If you want to find by name instead, you can use
        //   var item = await _inventoryRepository.GetByNameAsync(_inventoryDto.MaterialName);

        if (item is null)
            return false;

        item.ComponentName = _inventoryDto.MaterialName;
        item.MaterialCode = _inventoryDto.MaterialCode;
        item.QuantityAvailable = _inventoryDto.Quantity;
        item.MinimumStockLevel = _inventoryDto.MinimumStockLevel;
        item.UpdateAt = DateTime.UtcNow;
        item.UpdatedById = userId;


        await _inventoryRepository.UpdateAsync(item);
        return true;

       
    }
}
