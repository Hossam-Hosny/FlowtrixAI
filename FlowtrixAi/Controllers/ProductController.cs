using FlowtrixAI.Application.Product.Dtos;
using FlowtrixAI.Application.Product.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FlowtrixAI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProductController(IProductService _productService) : ControllerBase
    {

        /// <summary>
        /// Create Product and save to db
        /// </summary>
        /// <param name="createProductDto"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductDto createProductDto)
        {
            //var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
           var userId = 1;
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            

            var result = await _productService.CreateProductAsync(createProductDto, userId);

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
    }
}
