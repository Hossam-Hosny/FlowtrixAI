using FlowtrixAI.Application.Inventory.Interface;
using FlowtrixAI.Application.Production.Interface;
using FlowtrixAI.Domain.Entities;
using FlowtrixAI.Domain.Repositories;

namespace FlowtrixAI.Application.Production.Services;

internal class ProductionService(IProductionRecordRepository _productionRecordRepository,
    IBomRepository _bomRepository,IInventoryService _inventoryService)
    : IProductionService
{

    /// <summary>
    /// Checks the availability of components based on the Bill of Materials (BoM) for the specified product and quantity, deducts the required components from inventory, and saves a production record with the provided process and engineer details.
    /// </summary>
    /// <param name="productId">The ID of the product to be produced.</param>
    /// <param name="quantity">The quantity of the product to be produced.</param>
    /// <param name="processId">The ID of the production process.</param>
    /// <param name="engineerId">The ID of the engineer responsible for the production.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="Exception">Thrown when components are not available in the required quantity or no BoM is found for the product.</exception>

    public async Task CreateProductionAsync(int productId, decimal quantity, int processId, int engineerId)
    {
        // Get BoM for the product

        var bomItems = (await _bomRepository.GetAllAsync())
                            .Where(x=>x.ProductId == productId)
                            .ToList();

        if (!bomItems.Any())
            throw new Exception($"No BoM found for Product ID {productId}");

        // Check Availability of components in inventory
        foreach (var item in bomItems)
        {
            var requiredQuantity = item.QuantityRequired * quantity;

            var isAvailable = await _inventoryService.IsAvailableAsync(item.ComponentName, requiredQuantity);
            if (!isAvailable)
                throw new Exception($"Component '{item.ComponentName}' is not available in the required quantity of {requiredQuantity}.");
        }

        // Deduct components from inventory
        foreach (var item in bomItems)
        {
            var requiredQuantity = item.QuantityRequired * quantity;
            await _inventoryService.DeductAsync(item.ComponentName, requiredQuantity);
        }

        // Save production record
        var record = new ProductionRecord
        {
          ProcessId = processId,
            EngineerId = engineerId,
            QuantityProduced = quantity,
            ProduceAd = DateTime.UtcNow,
        };

        await _productionRecordRepository.AddAsync(record);
    }

    public async Task<IEnumerable<ProductionRecord>> GetAllAsync() => await _productionRecordRepository.GetAllAsync();

   
}
