using FlowtrixAI.Application.Reports.Dtos;

namespace FlowtrixAI.Application.Reports.Interface;

public interface IAiService
{
    Task<AiInsightDto> GenerateInsightsAsync();
}
