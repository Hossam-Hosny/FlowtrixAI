using FlowtrixAI.Domain.Entities;
using FlowtrixAI.Domain.Repositories;
using FlowtrixAI.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FlowtrixAI.Infrastructure.Repositories;

internal class QualityCheckRepository(AppDbContext _context) : IQualityCheckRepository
{
    public async Task<List<QualityCheck>> GetAllAsync()
        => await _context.QualityChecks.ToListAsync();

    public async Task<QualityCheck?> GetByIdAsync(int id)
        => await _context.QualityChecks.FirstOrDefaultAsync(x => x.Id == id);

    public async Task AddAsync(QualityCheck qc)
    {
        await _context.QualityChecks.AddAsync(qc);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(QualityCheck qc)
    {
        _context.QualityChecks.Update(qc);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var item = await _context.QualityChecks.FindAsync(id);
        if (item != null)
        {
            _context.QualityChecks.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
