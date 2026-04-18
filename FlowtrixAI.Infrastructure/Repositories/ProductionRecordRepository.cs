using FlowtrixAI.Domain.Entities;
using FlowtrixAI.Domain.Repositories;
using FlowtrixAI.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FlowtrixAI.Infrastructure.Repositories;

internal class ProductionRecordRepository(AppDbContext _context) : IProductionRecordRepository
{
    public async Task AddAsync(ProductionRecord record)
    {
        await _context.ProductionRecords.AddAsync(record);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var item = await _context.ProductionRecords.FindAsync(id);
        if (item != null)
        {
            _context.ProductionRecords.Remove(item);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<ProductionRecord>> GetAllAsync()
        => await _context.ProductionRecords
                            .Include(pr => pr.Process)
                            .ToListAsync();


    public async Task<ProductionRecord?> GetByIdAsync(int id)
        => await _context.ProductionRecords
                            .Include(pr => pr.Process)
                            .FirstOrDefaultAsync(pr => pr.Id == id);    



    public async Task UpdateAsync(ProductionRecord record)
    {
        _context.ProductionRecords.Update(record);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ProductionRecord>> GetLatestAsync(int count)
        => await _context.ProductionRecords
                            .Include(pr => pr.Process)
                            .OrderByDescending(pr => pr.ProduceAd)
                            .Take(count)
                            .ToListAsync();
}
