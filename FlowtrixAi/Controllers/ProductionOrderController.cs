using FlowtrixAI.Application.ProductionOrder.Dtos;
using FlowtrixAI.Application.ProductionOrder.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FlowtrixAI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductionOrderController(IProductionOrderService _productionOrderService) : ControllerBase
    {
        private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
       
        /// <summary>
        /// Creates a new production order with the specified product and quantity.
        /// </summary>
        /// <param name="createProductionOrderDto">An object containing the details of the production order to create, including the product identifier and
        /// quantity. Cannot be null.</param>
        /// <returns>An IActionResult containing the result of the creation operation. Returns HTTP 200 (OK) with the created
        /// production order details if successful.</returns>

        [HttpPost("create")]
        public async Task<IActionResult> CreateProductionOrder(CreateProductionOrderDto createProductionOrderDto)
        {
            var result = await _productionOrderService.CreateOrderAsync(createProductionOrderDto.ProductId, createProductionOrderDto.Quantity, CurrentUserId);
            return Ok(result);
        }
        
        /// <summary>
        /// Starts the production order with the specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the production order to start.</param>
        /// <returns>An <see cref="IActionResult"/> that represents the result of the operation. Returns a success response with
        /// the result of the start operation.</returns>

        [HttpPost("{id}/start")]
        public async Task<IActionResult> Start(int id)
        {
            var result = await _productionOrderService.StartOrderAsync(id);
            return Ok(result);
        }
        /// <summary>
        /// Marks the specified production order as complete.
        /// </summary>
        /// <param name="id">The unique identifier of the production order to complete.</param>
        /// <returns>An IActionResult that represents the result of the operation. Returns a 200 OK response with the completion
        /// result if successful.</returns>

        [HttpPost("{id}/complete")]
        public async Task<IActionResult> Complete(int id)
        {
            var result = await _productionOrderService.CompleteOrderAsync(id);
            return Ok(result);
        }
        /// <summary>
        /// Marks the specified production order as failed.
        /// </summary>
        /// <param name="id">The unique identifier of the production order to mark as failed.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IActionResult"/>
        /// indicating the outcome of the operation.</returns>

        [HttpPost("{id}/fail")]
        public async Task<IActionResult> Fail(int id, [FromBody] FailOrderRequest request)
        {
            var result = await _productionOrderService.FailOrderAsync(id, request.ProblemDescription, CurrentUserId);
            return Ok(result);
        }
        /// <summary>
        /// Marks the specified production order as delivered.
        /// </summary>
        /// <param name="id">The unique identifier of the production order to deliver.</param>
        /// <returns>An IActionResult containing the result of the delivery operation.</returns>

        [HttpPost("{id}/deliver")]
        public async Task<IActionResult> Deliver(int id)
        {
            var result = await _productionOrderService.DeliverOrderAsync(id);
            return Ok(result);
        }

        [HttpPost("{id}/progress")]
        public async Task<IActionResult> UpdateProgress(int id, [FromQuery] int stepIndex)
        {
            var result = await _productionOrderService.UpdateOrderProgressAsync(id, stepIndex);
            return Ok(result);
        }
        /// <summary>
        /// Retrieves the total number of completed production orders.
        /// </summary>
        /// <returns>An HTTP 200 response containing the number of completed orders if any exist; otherwise, an HTTP 400 response
        /// indicating that there are no completed orders.</returns>

        // get all Completed Orders
        [HttpGet("CompletedOrdersNo")]
        public async Task<IActionResult> GetNumberOfAllCompletedOrders()
        {
            var result = await _productionOrderService.GetNumberOfAllCompletedOrders();
            if (result == 0)
                return BadRequest(" 0 => there is no Completed Orders Yet!");

             return Ok(result);
        }
        /// <summary>
        /// Retrieves the total number of failed production orders.
        /// </summary>
        /// <remarks>Use this endpoint to monitor the count of failed production orders. Returns a 400 Bad
        /// Request response if there are no failed orders.</remarks>
        /// <returns>An <see cref="IActionResult"/> containing the number of failed orders if any exist; otherwise, a bad request
        /// result indicating that there are no failed orders.</returns>

        // get all Fail Orders
        [HttpGet("FaildOrdersNo")]
        public async Task<IActionResult> GetNumberOfAllFaildOrders()
        {
            var result = await _productionOrderService.GetNumberOfAllFaildOrders();
            if (result == 0)
                return BadRequest(" 0 => there is no Faild Orders Yet!");

             return Ok(result);
        }

        /// <summary>
        /// Retrieves all production orders.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the list of all production orders if any exist; otherwise, a
        /// NotFound result if no production orders are found.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _productionOrderService.GetOrderByIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _productionOrderService.GetAllOrdersAsync();
            if (result == null || !result.Any())
                return NotFound("No Production Orders Found");
            return Ok(result);
        }






        [HttpDelete("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            var result = await _productionOrderService.CancelOrderAsync(id);
            return Ok(result);
        }
    }
}

