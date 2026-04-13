using FlowtrixAI.Application.Reports.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FlowtrixAI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController(IReportService _reportService) : ControllerBase
    {

        [HttpGet("[action]")]
        public async Task<IActionResult> GetReport()
        {
            var result = await _reportService.GetSystemReportAsync();
            return Ok(result);
        }


    }
}
