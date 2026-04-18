using FlowtrixAI.Application.Production.Dtos;
using FlowtrixAI.Domain.Entities;
using FlowtrixAI.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FlowtrixAI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductionController(IProductionRecordRepository _productionRecordRepository) : ControllerBase
    {

        // record production data
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductionDto createProductionDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var record = new ProductionRecord
            {
                ProcessId = createProductionDto.ProcessId,
                QuantityProduced = createProductionDto.QuantityProduced,
                ProduceAd = createProductionDto.Date,
                EngineerId = userId
            };

            await _productionRecordRepository.AddAsync(record);

            return Ok(record);
        }

        // get all production records
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var records = await _productionRecordRepository.GetAllAsync();
            return Ok(records);



        }

        // get production records for a specific product
        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetByProduct(int productId)
        {
            var records = await _productionRecordRepository.GetByIdAsync(productId);
            return Ok(records);
        }
    }
}