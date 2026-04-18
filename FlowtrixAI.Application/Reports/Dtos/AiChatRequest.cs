using System.Collections.Generic;

namespace FlowtrixAI.Application.Reports.Dtos;

public class AiChatRequest
{
    public string Message { get; set; } = string.Empty;
    public string ChatId { get; set; } = string.Empty; // معرف المحادثة الحالية
    public List<AiChatMessageDto> History { get; set; } = new();
}

public class AiChatMessageDto
{
    public string Role { get; set; } = string.Empty; // "user" or "model"
    public string Text { get; set; } = string.Empty;
}
