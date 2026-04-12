using FlowtrixAI.Application.Product.Dtos;
using FlowtrixAI.Domain.Entities;
using FlowtrixAI.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FlowtrixAI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProductRepository _productRepository) : ControllerBase
    {

        // Create Product
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto createProductDto)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");

             if (!Directory.Exists(folderPath))
              {
                    Directory.CreateDirectory(folderPath);
              }

             var fileName = Guid.NewGuid()+Path.GetExtension(createProductDto.Image.FileName);
            var fullPath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                    await createProductDto.Image.CopyToAsync(stream);
            }





            var product = new Product
            {
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                ImagePath =$"/images/products/{fileName}",
            };

            await _productRepository.AddAsync(product);
            return Ok(product);


        }
        // Get All Products
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productRepository.GetAllAsync();

            return Ok(products);


        }

        // Get Product by Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
    }
}
