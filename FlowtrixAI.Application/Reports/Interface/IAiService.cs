using FlowtrixAI.Application.Reports.Dtos;

namespace FlowtrixAI.Application.Reports.Interface;

public interface IAiService
{
    Task<AiInsightDto> GenerateInsightsAsync();
    Task<string> GenerateFullReportAsync();
    Task<string> ChatWithAiAsync(string userId, string chatId, List<FlowtrixAI.Application.Reports.Dtos.AiChatMessageDto> history, string userMessage);
    Task<IEnumerable<FlowtrixAI.Domain.Entities.AiChatHistory>> GetUserChatSessionsAsync(string userId);
    Task<IEnumerable<FlowtrixAI.Domain.Entities.AiChatHistory>> GetChatMessagesBySessionAsync(string chatId);
    Task DeleteChatSessionAsync(string chatId);
}
