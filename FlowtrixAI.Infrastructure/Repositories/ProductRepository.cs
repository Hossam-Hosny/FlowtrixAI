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

    public async Task<List<Product>> GetAllAsync() => await _context.Products.Include(p => p.BOMs)
    .Select(p => new Product
    {
        Id = p.Id,
        Name = p.Name,
        Description = p.Description,
        ImagePath = p.ImagePath,
        CreatedBy = p.CreatedBy,
        CreatedAt = p.CreatedAt,
        BOMs = p.BOMs.Select(b => new BillOfMaterial
        {
            Id = b.Id,
            ComponentName = b.ComponentName,
            QuantityRequired = b.QuantityRequired,
            Unit = b.Unit
            
        }).ToList()
    })
        .ToListAsync();

    public async Task<Product?> GetByIdAsync(int id) => await _context.Products.Include(p => p.BOMs)
        .Select(p => new Product
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            ImagePath = p.ImagePath,
            CreatedBy = p.CreatedBy,
            CreatedAt = p.CreatedAt,
            BOMs = p.BOMs.Select(b => new BillOfMaterial
            {
                Id =b.Id,
                ComponentName = b.ComponentName,
                QuantityRequired = b.QuantityRequired,
                Unit = b.Unit
                
            }).ToList()

        }).FirstOrDefaultAsync(p => p.Id == id);


    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }
}
