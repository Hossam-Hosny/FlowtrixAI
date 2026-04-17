using FlowtrixAI.Domain.Entities;
using FlowtrixAI.Domain.Repositories;
using FlowtrixAI.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FlowtrixAI.Infrastructure.Repositories;

internal class ProductRepository(AppDbContext _context) : IProductRepository
{
    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
       var product = _context.Products.FirstOrDefault(p => p.Id == id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Product>> GetAllAsync() => await _context.Products
        .Include(p => p.BOMs)
        .Include(p => p.Processes)
        .ToListAsync();

    public async Task<Product?> GetByIdAsync(int id) => await _context.Products
        .Include(p => p.BOMs)
        .Include(p => p.Processes)
        .FirstOrDefaultAsync(p => p.Id == id);


    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }
}
