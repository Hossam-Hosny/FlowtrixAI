using FlowtrixAI.Domain.Entities;
using FlowtrixAI.Domain.Repositories;
using FlowtrixAI.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FlowtrixAI.Infrastructure.Repositories;

internal class InventoryRepository(AppDbContext _context) : IInventoryRepository
{
    public async Task AddAsync(InventoryItem item)
    {
        await _context.Inventory.AddAsync(item);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var item = await _context.Inventory.FindAsync(id);
        if (item != null)
        {
            _context.Inventory.Remove(item);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<InventoryItem>> GetAllAsync()=> await _context.Inventory.ToListAsync();

    public async Task<InventoryItem?> GetByIdAsync(int id) => await _context.Inventory.FirstOrDefaultAsync(i => i.Id == id);

    public async Task<InventoryItem?> GetByNameAsync(string name) => await _context.Inventory.FirstOrDefaultAsync(i => i.ComponentName == name);


    public async Task UpdateAsync(InventoryItem item)
    {
        _context.Inventory.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<InventoryItem>> GetTopAsync(int count)
        => await _context.Inventory.OrderByDescending(i => i.Id).Take(count).ToListAsync();
}
