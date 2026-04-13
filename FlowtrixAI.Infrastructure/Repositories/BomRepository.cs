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
    /// <summary>
    /// retreives a list of BillOfMaterial for a specified Product Id. This is because a product can have multiple components in its bill of materials, so we return a list to accommodate all related entries.
    /// </summary>
    /// <param name="id">The unique identifier of the product for which to retrieve the bill of materials.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of bill of material entities
    /// for the specified product, or an empty list if no matching records are found.</returns>
    public async Task<List<BillOfMaterial>?> GetByIdAsync(int id)=> await _context.BoMs.Where(b => b.Id == id).ToListAsync();


    public async Task UpdateAsync(BillOfMaterial bom)
    {
        _context.BoMs.Update(bom);
        await _context.SaveChangesAsync();
    }
}
