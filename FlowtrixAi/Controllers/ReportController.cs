using FlowtrixAI.Application.Reports.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlowtrixAI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ReportController(IReportService _reportService, IAiService _aiService) : ControllerBase
    {
        [HttpGet("[action]")]
        public async Task<IActionResult> GetReport()
        {
            var result = await _reportService.GetSystemReportAsync();
            return Ok(result);
        }

        [HttpGet("ai-full-report")]
        public async Task<IActionResult> GetAiFullReport()
        {
            var result = await _aiService.GenerateFullReportAsync();
            return Ok(new { report = result });
        }

        [HttpPost("ai-chat")]
        public async Task<IActionResult> ChatWithAi([FromBody] FlowtrixAI.Application.Reports.Dtos.AiChatRequest request)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";
            var result = await _aiService.ChatWithAiAsync(userId, request.ChatId, request.History, request.Message);
            return Ok(new { reply = result });
        }

        [HttpGet("ai-chat-sessions")]
        public async Task<IActionResult> GetUserChatSessions()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";
            var sessions = await _aiService.GetUserChatSessionsAsync(userId);
            return Ok(sessions);
        }

        [HttpGet("ai-chat-messages/{chatId}")]
        public async Task<IActionResult> GetChatMessagesBySession(string chatId)
        {
            var messages = await _aiService.GetChatMessagesBySessionAsync(chatId);
            return Ok(messages);
        }

        [HttpPost("delete-session/{chatId}")]
        public async Task<IActionResult> DeleteChatSession([FromRoute] string chatId)
        {
            await _aiService.DeleteChatSessionAsync(chatId);
            return Ok(new { success = true });
        }
    }
}
