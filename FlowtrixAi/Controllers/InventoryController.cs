using FlowtrixAI.Application.Inventory.Dtos;
using FlowtrixAI.Domain.Entities;
using FlowtrixAI.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FlowtrixAI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController(IInventoryRepository _inventoryRepository) : ControllerBase
    {

        
        /// <summary>
        ///  add a new material to the inventory. The request body should contain the material name, quantity, and unit. The endpoint will create a new inventory item with the provided details and return the created item in the response.
        /// </summary>
        /// <param name="_inventoryDto">The DTO containing the details of the material to be added to the inventory.</param>
        /// <returns>The created inventory item.</returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> AddToInventory(CreateInventoryDto _inventoryDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);



            var item = new InventoryItem
            {
                ComponentName = _inventoryDto.MaterialName,
                QuantityAvailable = _inventoryDto.Quantity,
                Unit = _inventoryDto.unit.ToString(),
                UpdatedById = userId,
                UpdateAt = DateTime.UtcNow,

            };

            await _inventoryRepository.AddAsync(item);
            return Ok(item);


        }

        /// <summary>
        /// Retrieves all inventory items.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the collection of inventory items. The result is an HTTP 200
        /// response with the items if successful.</returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllInventory()
        {
            var inventoryItems = await _inventoryRepository.GetAllAsync();
            return Ok(inventoryItems);

        }

        
        /// <summary>
        /// Updates the quantity of an existing inventory item based on the provided data.
        /// </summary>
        /// <remarks>This method requires authentication. The update records the user performing the
        /// operation and the time of the update.</remarks>
        /// <param name="_inventoryDto">An object containing the inventory item's identifier and the new quantity to set. The identifier must
        /// correspond to an existing inventory item.</param>
        /// <returns>An HTTP 200 response with the updated inventory item if the update is successful; otherwise, an HTTP 404
        /// response if the item is not found.</returns>
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateInventory(UpdateInventoryDto _inventoryDto)
        {
            var item = await _inventoryRepository.GetByIdAsync(_inventoryDto.ComponentId);

            // If you want to find by name instead, you can use
           //   var item = await _inventoryRepository.GetByNameAsync(_inventoryDto.MaterialName);

            if (item == null)
            {
                return NotFound();
            }

            item.QuantityAvailable = _inventoryDto.Quantity;
            item.UpdateAt = DateTime.UtcNow;
            item.UpdatedById = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await _inventoryRepository.UpdateAsync(item);
            return Ok(item);
        }
    }
}
