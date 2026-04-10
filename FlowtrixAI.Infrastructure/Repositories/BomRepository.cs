using FlowtrixAI.Domain.Entities;
using FlowtrixAI.Domain.Repositories;
using FlowtrixAI.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FlowtrixAI.Infrastructure.Repositories;

internal class BomRepository(AppDbContext _context) : IBomRepository
{
    public async Task AddAsync(BillOfMaterial bom)
    {
        await _context.BoMs.AddAsync(bom);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var item = _context.BoMs.Find(id);
        if (item != null)
        {
            _context.BoMs.Remove(item);
            await _context.SaveChangesAsync();

        }
        
    }

    public async Task<IEnumerable<BillOfMaterial>> GetAllAsync()=> await _context.BoMs.ToListAsync();


    public async Task<BillOfMaterial?> GetByIdAsync(int id)=> await _context.BoMs.FirstOrDefaultAsync(b => b.Id == id);


    public async Task UpdateAsync(BillOfMaterial bom)
    {
        _context.BoMs.Update(bom);
        await _context.SaveChangesAsync();
    }
}
