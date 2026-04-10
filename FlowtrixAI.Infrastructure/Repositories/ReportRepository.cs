using FlowtrixAI.Domain.Entities;
using FlowtrixAI.Domain.Repositories;
using FlowtrixAI.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FlowtrixAI.Infrastructure.Repositories;

internal class ReportRepository(AppDbContext _context) : IReportRepository
{
    public async Task AddAsync(Report report)
    {
        await _context.Reports.AddAsync(report);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var item =await _context.Reports.FindAsync(id);
        if (item != null)
        {
            _context.Reports.Remove(item);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Report>> GetAllAsync()=> await _context.Reports.Include(x=>x.GeneratedBy).ToListAsync();
  

    public async Task<Report?> GetByIdAsync(int id)=> await _context.Reports.FirstOrDefaultAsync(x=>x.Id == id);
  

    public async Task UpdateAsync(Report report)
    {
        _context.Reports.Update(report);
        await _context.SaveChangesAsync();
    }
}
