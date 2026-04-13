using FlowtrixAI.Application.ProductionOrder.Dtos;
using FlowtrixAI.Application.ProductionOrder.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FlowtrixAI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ProductionOrderController(IProductionOrderService _productionOrderService) : ControllerBase
    {

        [HttpPost("create")]
        public async Task<IActionResult> CreateProductionOrder(CreateProductionOrderDto createProductionOrderDto)
        {
          //  var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userId = 3; // Temporary hardcoded user ID for testing purposes

            var result = await _productionOrderService.CreateOrderAsync(createProductionOrderDto.ProductId, createProductionOrderDto.Quantity, userId);
            return Ok(result);

        }


        [HttpPost("{id}/start")]
        public async Task<IActionResult> Start(int id)
        {
            var result = await _productionOrderService.StartOrderAsync(id);
            return Ok(result);
        }

        [HttpPost("{id}/complete")]
        public async Task<IActionResult> Complete(int id)
        {
            var result = await _productionOrderService.CompleteOrderAsync(id);
            return Ok(result);
        }

        [HttpPost("{id}/fail")]
        public async Task<IActionResult> Fail(int id)
        {
            var result = await _productionOrderService.FailOrderAsync(id);
            return Ok(result);
        }

        [HttpPost("{id}/deliver")]
        public async Task<IActionResult> Deliver(int id)
        {
            var result = await _productionOrderService.DeliverOrderAsync(id);
            return Ok(result);
        }

        // get all Completed Products
        [HttpGet("CompletedOrdersNo")]
        public async Task<IActionResult> GetNumberOfAllCompletedOrders()
        {
            var result = await _productionOrderService.GetNumberOfAllCompletedOrders();
            if (result == 0)
                return BadRequest("there is no Completed Orders Yet!");

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






    }
}

