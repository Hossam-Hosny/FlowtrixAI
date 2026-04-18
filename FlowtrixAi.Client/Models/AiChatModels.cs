namespace FlowtrixAi.Client.Models;

public class AiChatRequest
{
    public string Message { get; set; } = string.Empty;
    public string ChatId { get; set; } = string.Empty;
    public List<AiChatMessageDto> History { get; set; } = new();
}

public class AiChatMessageDto
{
    public string Role { get; set; } = string.Empty; // "user" or "model"
    public string Text { get; set; } = string.Empty;
}

public class AiChatResponse
{
    public string Reply { get; set; } = string.Empty;
}

public class AiReportResponse
{
    public string Report { get; set; } = string.Empty;
}
