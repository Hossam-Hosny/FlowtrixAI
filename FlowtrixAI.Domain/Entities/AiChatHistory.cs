using System;
using System.ComponentModel.DataAnnotations;

namespace FlowtrixAI.Domain.Entities;

public class AiChatHistory
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public string UserMessage { get; set; } = string.Empty;

    [Required]
    public string AiResponse { get; set; } = string.Empty;

    public string ChatId { get; set; } = string.Empty; // معرف الجلسة لتجميع الرسائل

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    // اوبشن اختيارى لمعرفة نوع التفاعل (Chat vs Report)
    public string InteractionType { get; set; } = "Chat";
}
