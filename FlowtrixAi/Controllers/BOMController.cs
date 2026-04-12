using FlowtrixAI.Application.BoM.Dtos;
using FlowtrixAI.Domain.Entities;
using FlowtrixAI.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FlowtrixAI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BOMController(IBomRepository _bomRepository) : ControllerBase
    {

        // Add Component to BoM
        [HttpPost]
        public async Task<IActionResult> AddComponentToBoM(AddBoMdto addBoMdto)
        {
            var bom = new BillOfMaterial
            {
                ProductId = addBoMdto.ProductId,
                ComponentName = addBoMdto.ComponentName,
                QuantityRequired = addBoMdto.Quantity,
            };

            await _bomRepository.AddAsync(bom);
            return Ok(bom);



        }

        // Get BoM for a Product
        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetBoMByProductId(int productId)
        {
            var bom = await _bomRepository.GetByIdAsync(productId);

            if (bom == null)
            {
                return NotFound();
            }
            return Ok(bom);

        }


    }
}
