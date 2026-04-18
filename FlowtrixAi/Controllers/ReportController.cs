using FlowtrixAI.Application.Reports.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlowtrixAI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportController(IReportService _reportService) : ControllerBase
    {
      /// <summary>
      /// Handles HTTP GET requests to retrieve the current system report.
      /// </summary>
      /// <remarks>The returned result typically includes system status and diagnostic information. The
      /// exact structure of the report is determined by the implementation of the report service.</remarks>
      /// <returns>An <see cref="IActionResult"/> containing the system report data if successful.</returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetReport()
        {
            var result = await _reportService.GetSystemReportAsync();
            return Ok(result);
        }




    }
}
