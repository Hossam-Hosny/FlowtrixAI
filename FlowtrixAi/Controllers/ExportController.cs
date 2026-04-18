using FlowtrixAI.Application.Export.Dtos;
using FlowtrixAI.Application.Export.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlowtrixAI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExportController : ControllerBase
    {
        private readonly IExportService _exportService;

        public ExportController(IExportService exportService)
        {
            _exportService = exportService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExportDto>>> GetExports()
        {
            var exports = await _exportService.GetAllExportsAsync();
            return Ok(exports);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExportDto>> GetExport(int id)
        {
            var export = await _exportService.GetExportByIdAsync(id);
            if (export == null) return NotFound();
            return Ok(export);
        }

        [HttpPost]
        public async Task<ActionResult> CreateExport([FromBody] CreateExportDto createExportDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _exportService.CreateExportAsync(createExportDto);
            if (!result) return BadRequest("Failed to create export.");

            return Ok("Export created successfully.");
        }

        [HttpPatch("{id}/status")]
        public async Task<ActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            var result = await _exportService.UpdateExportStatusAsync(id, status);
            if (!result) return BadRequest("Failed to update status.");

            return Ok("Status updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteExport(int id)
        {
            var result = await _exportService.DeleteExportAsync(id);
            if (!result) return NotFound();

            return Ok("Export deleted successfully.");
        }
    }
}
