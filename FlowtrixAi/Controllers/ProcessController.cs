using FlowtrixAI.Application.Process.Dtos;
using FlowtrixAI.Domain.Entities;
using FlowtrixAI.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlowtrixAI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProcessController(IProcessRepository _processRepository) : ControllerBase
    {

        // Add Process Step
        [HttpPost]
        public async Task<IActionResult> AddStep(CreateProcessDto createProcessDto)
        {
            var process = new Process
            {
                ProductId = createProcessDto.ProductId,
                StepName = createProcessDto.StepName,
                DurationMinutes = createProcessDto.EstimatedTimeInHours * 60,
            };

            await _processRepository.AddAsync(process);
            return Ok(process);
        }

        // Get Process Steps for a Product
        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetProcessByProductId(int productId)
        {
            var processSteps = await _processRepository.GetByIdAsync(productId);
            if (processSteps == null)
            {
                return NotFound();
            }

            return Ok(processSteps);



        }
    }
}
