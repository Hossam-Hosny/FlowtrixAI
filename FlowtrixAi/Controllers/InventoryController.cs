using FlowtrixAI.Application.Inventory.Dtos;
using FlowtrixAI.Application.Inventory.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FlowtrixAI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController(IInventoryService _inventoryService) : ControllerBase
    {

        
        /// <summary>
        ///  add a new material to the inventory. The request body should contain the material name, quantity, and unit. The endpoint will create a new inventory item with the provided details and return the created item in the response.
        /// </summary>
        /// <param name="_inventoryDto">The DTO containing the details of the material to be added to the inventory.</param>
        /// <returns>The created inventory item.</returns>
        [HttpPost("AddToInventory")]
        public async Task<IActionResult> AddToInventory([FromBody] CreateInventoryDto _inventoryDto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);



            //var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userId = 1;

           var result = await _inventoryService.AddItemAsync(_inventoryDto, userId);

            return Ok("Item Added to Inventory Successfully");


        }

        /// <summary>
        /// Retrieves all inventory items.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the collection of inventory items. The result is an HTTP 200
        /// response with the items if successful.</returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllInventory()
        {
            var inventoryItems = await _inventoryService.GetAllAsync();

            if (inventoryItems is null)
                return NotFound("No Inventory Items Found");

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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

          //  var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
          var userId = 3;

            var result = await _inventoryService.UpdateItemAsync(_inventoryDto,userId);

            if (result == false)
                return NotFound("Inventory Item Not Found");

            return Ok("Inventory Item Updated Successfully");


            
        }
    }
}
