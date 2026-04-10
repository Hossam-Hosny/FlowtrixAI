using FlowtrixAI.Domain.Entities;
using FlowtrixAI.Domain.Repositories;
using FlowtrixAI.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FlowtrixAI.Infrastructure.Repositories;

internal class ProcessRepository(AppDbContext _context) : IProcessRepository
{
    public async Task AddAsync(Process process)
    {
        await _context.Processes.AddAsync(process);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var item = await _context.Processes.FindAsync(id);
        if (item != null)
        {
            _context.Processes.Remove(item);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Process>> GetAllAsync() => await _context.Processes.ToListAsync();


    public async Task<Process?> GetByIdAsync(int id)=> await _context.Processes.FirstOrDefaultAsync(p => p.Id == id);


    public async Task UpdateAsync(Process process)
    {
        _context.Processes.Update(process);
        await _context.SaveChangesAsync();
    }
}
