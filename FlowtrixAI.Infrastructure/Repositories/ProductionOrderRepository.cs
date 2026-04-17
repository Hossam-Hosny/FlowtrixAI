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

    public async Task<List<ProductionOrder>> GetAllAsync()=> await _context.ProductionOrders.Include(o=>o.Product).ThenInclude(p=>p.Processes).Include(o=>o.ReportedBy).ToListAsync();

    public async Task<IEnumerable<ProductionOrder>> GetAllCompletedOrders()=>await _context.ProductionOrders.Include(o=>o.Product).Where(x=>x.Status==OrderSteps.Completed).ToListAsync();

    public async Task<IEnumerable<ProductionOrder>> GetAllFaildOrdersAsync() => await _context.ProductionOrders.Where(o => o.Status == OrderSteps.Rejected).ToListAsync();
   

    public async Task<ProductionOrder?> GetByIdAsync(int id)=> await _context.ProductionOrders.Include(o=>o.Product).FirstOrDefaultAsync(o=>o.Id==id);


    public async Task UpdateAsync(ProductionOrder order)
    {
        _context.ProductionOrders.Update(order);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(ProductionOrder order)
    {
        _context.ProductionOrders.Remove(order);
        await _context.SaveChangesAsync();
    }
}
