using FlowtrixAI.Domain.Entities;
using FlowtrixAI.Domain.Repositories;
using FlowtrixAI.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FlowtrixAI.Infrastructure.Repositories
{
    public class ExportRepository : IExportRepository
    {
        private readonly AppDbContext _context;

        public ExportRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Export>> GetAllAsync()
        {
            return await _context.Exports
                .Include(e => e.Product)
                .OrderByDescending(e => e.OrderDate)
                .ToListAsync();
        }

        public async Task<Export?> GetByIdAsync(int id)
        {
            return await _context.Exports
                .Include(e => e.Product)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Export> AddAsync(Export export)
        {
            _context.Exports.Add(export);
            await _context.SaveChangesAsync();
            return export;
        }

        public async Task UpdateAsync(Export export)
        {
            _context.Exports.Update(export);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var export = await _context.Exports.FindAsync(id);
            if (export != null)
            {
                _context.Exports.Remove(export);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Export>> GetLatestAsync(int count)
        {
            return await _context.Exports
                .Include(e => e.Product)
                .OrderByDescending(e => e.OrderDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Export>> GetByDateRangeAsync(DateTime start, DateTime end)
        {
            return await _context.Exports
                .Include(e => e.Product)
                .Where(e => e.OrderDate >= start && e.OrderDate <= end)
                .OrderByDescending(e => e.OrderDate)
                .ToListAsync();
        }
    }
}
