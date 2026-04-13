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
        /// <summary>
        /// Add a component to the Bill of Materials for a specific product. This endpoint allows you to specify the product, component name, quantity required, and unit of measurement. The component will be added to the BoM for the specified product.
        /// </summary>
        /// <param name="addBoMdto">The DTO containing the details of the component to be added to the BoM.</param>
        /// <returns>The added BoM component.</returns>

        [HttpPost]
        public async Task<IActionResult> AddBoMsForProduct(AddBoMdto addBoMdto)
        {
            var bom = new BillOfMaterial
            {
                ProductId = addBoMdto.ProductId,
                ComponentName = addBoMdto.ComponentName,
                QuantityRequired = addBoMdto.Quantity,
                Unit = addBoMdto.Unit.ToString()
            };

            await _bomRepository.AddAsync(bom);
            return Ok(bom);



        }

        // Get BoM for a Product
        /// <summary>
        /// Retrieve the Bill of Materials for a specific product. This endpoint allows you to get all components and their details for the specified product.
        /// </summary>
        /// <param name="productId">The ID of the product for which to retrieve the BoM.</param>
        /// <returns>The BoM components for the specified product.</returns>
        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetBoMsForProduct(int productId)
        {
            var boms = await _bomRepository.GetByIdAsync(productId);

            if (boms == null)
            {
                return NotFound();
            }
            return Ok(boms);

        }


    }
}
