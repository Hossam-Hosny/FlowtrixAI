using FlowtrixAI.Domain.Entities;
using FlowtrixAI.Domain.Repositories;
using FlowtrixAI.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlowtrixAI.Infrastructure.Repositories;

internal class AiChatRepository(AppDbContext _context) : IAiChatRepository
{
    public async Task AddHistoryAsync(AiChatHistory history)
    {
        await _context.AiChatHistories.AddAsync(history);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<AiChatHistory>> GetUserHistoryAsync(string userId, int count = 50)
    {
        return await _context.AiChatHistories
            .Where(h => h.UserId == userId)
            .OrderByDescending(h => h.Timestamp)
            .Take(count)
            .ToListAsync();
    }

    public async Task<IEnumerable<AiChatHistory>> GetMessagesByChatIdAsync(string chatId)
    {
        return await _context.AiChatHistories
            .Where(h => h.ChatId == chatId)
            .OrderBy(h => h.Timestamp)
            .ToListAsync();
    }

    public async Task<IEnumerable<AiChatHistory>> GetUserChatSessionsAsync(string userId)
    {
        var allHistory = await _context.AiChatHistories
            .Where(h => h.UserId == userId)
            .ToListAsync();

        var sessions = allHistory
            .GroupBy(h => h.ChatId)
            .Select(g => g.OrderByDescending(x => x.Timestamp).First())
            .OrderByDescending(x => x.Timestamp)
            .ToList();

        return sessions;
    }

    public async Task DeleteChatSessionAsync(string chatId)
    {
        await _context.AiChatHistories
            .Where(h => h.ChatId == chatId)
            .ExecuteDeleteAsync();
    }
}
