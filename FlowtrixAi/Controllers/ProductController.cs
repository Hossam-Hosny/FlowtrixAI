using FlowtrixAI.Application.Product.Dtos;
using FlowtrixAI.Application.Product.Interface;
using FlowtrixAI.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FlowtrixAI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController(IProductService _productService) : ControllerBase
    {
        private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        /// <summary>
        /// Create Product and save to db
        /// </summary>
        /// <param name="createProductDto"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto createProductDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _productService.CreateProductAsync(createProductDto, CurrentUserId);

            if (result==false)
                return BadRequest("Failed to create product.");

            return Ok("Product Created Successfully");
        }
       
        /// <summary>
        /// Retrieves all available products.   
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the list of products if any exist; otherwise, a NotFound result if
        /// no products are found.</returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _productService.GetAllProductsAsync();
            if (result == null || !result.Any())
                return NotFound("No Products found.");
            
            return Ok(result);
        }

        // Get Product by Id
        /// <summary>
        /// Retrieves the product with the specified identifier.    
        /// </summary>
        /// <param name="id">The unique identifier of the product to retrieve.</param>
        /// <returns>An IActionResult containing the product data if found; otherwise, a NotFound result if no product exists
        /// with the specified identifier.</returns>
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var result = await _productService.GetProductByIdAsync(id);

            if (result == null)
                return NotFound("Product not Found");
            
            return Ok(result);
        }

        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] CreateProductDto updateProductDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _productService.UpdateProductAsync(id, updateProductDto, CurrentUserId);

            if (!result)
                return BadRequest("Failed to update product.");

            return Ok("Product Updated Successfully");
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);

            if (!result)
                return BadRequest("Failed to delete product or product not found.");

            return Ok("Product Deleted Successfully");
        }
    }
}
