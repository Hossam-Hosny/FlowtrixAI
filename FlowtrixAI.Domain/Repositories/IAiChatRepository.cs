using FlowtrixAI.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlowtrixAI.Domain.Repositories;

public interface IAiChatRepository
{
    Task AddHistoryAsync(AiChatHistory history);
    Task<IEnumerable<AiChatHistory>> GetUserHistoryAsync(string userId, int count = 50);
    Task<IEnumerable<AiChatHistory>> GetMessagesByChatIdAsync(string chatId);
    Task<IEnumerable<AiChatHistory>> GetUserChatSessionsAsync(string userId);
    Task DeleteChatSessionAsync(string chatId);
}
