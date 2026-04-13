using FlowtrixAI.Domain.Constants;
using FlowtrixAI.Domain.Entities;
using FlowtrixAI.Domain.Repositories;
using FlowtrixAI.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FlowtrixAI.Infrastructure.Repositories;

internal class ProductionOrderRepository(AppDbContext _context) : IProductionOrderRepository
{
    public async Task AddAsync(ProductionOrder order)
    {
        await _context.ProductionOrders.AddAsync(order);
        await _context.SaveChangesAsync();
    }

    public async Task<List<ProductionOrder>> GetAllAsync()=> await _context.ProductionOrders.Include(o=>o.Product).ToListAsync();

    public async Task<IEnumerable<ProductionOrder>> GetAllCompletedOrders()=>await _context.ProductionOrders.Include(o=>o.Product).Where(x=>x.Status==OrderSteps.Completed).ToListAsync();
    

    public async Task<ProductionOrder?> GetByIdAsync(int id)=> await _context.ProductionOrders.Include(o=>o.Product).FirstOrDefaultAsync(o=>o.Id==id);


    public async Task UpdateAsync(ProductionOrder order)
    {
        _context.ProductionOrders.Update(order);
        await _context.SaveChangesAsync();
    }
}
