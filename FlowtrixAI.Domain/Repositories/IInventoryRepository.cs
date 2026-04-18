using FlowtrixAI.Domain.Entities;

namespace FlowtrixAI.Domain.Repositories;

public interface IInventoryRepository
{
    Task<IEnumerable<InventoryItem>> GetAllAsync();
    Task<InventoryItem?> GetByIdAsync(int id);
    Task<InventoryItem?> GetByNameAsync(string name);
    Task AddAsync(InventoryItem item);
    Task UpdateAsync(InventoryItem item);
    Task DeleteAsync(int id);
    Task<IEnumerable<InventoryItem>> GetTopAsync(int count);
}
