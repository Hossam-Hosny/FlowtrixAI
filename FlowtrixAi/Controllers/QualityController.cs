using FlowtrixAI.Application.Quality.Dtos;
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
    public class QualityController(IQualityCheckRepository _qualityCheckRepository) : ControllerBase
    {
        //Add Quality Check Result
       [HttpPost]
        public async Task<IActionResult> Create(CreateQualityDto createQualityDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            
            var quality = new QualityCheck
            {
                ProductionRecordId = createQualityDto.ProductionRecordId,
               
                DecfectCount = createQualityDto.DefectsCount,
                Notes = createQualityDto.Notes,
                CheckedById = userId,
                CheckAt = DateTime.UtcNow
            };

            await _qualityCheckRepository.AddAsync(quality);
            return Ok (quality);


        }
        // Get Quality Check Results for a Production Record
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var qualityChecks = await _qualityCheckRepository.GetAllAsync();
            return Ok(qualityChecks);
        }




    }
}
