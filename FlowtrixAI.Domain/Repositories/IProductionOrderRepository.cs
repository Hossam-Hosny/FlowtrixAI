using FlowtrixAI.Domain.Entities;

namespace FlowtrixAI.Domain.Repositories;

public interface IProductionOrderRepository
{
    Task AddAsync(ProductionOrder order);
    Task <ProductionOrder?> GetByIdAsync(int id);
    Task<List<ProductionOrder>> GetAllAsync();
    Task UpdateAsync (ProductionOrder order);
    Task<IEnumerable<ProductionOrder>> GetAllCompletedOrders();
    Task<IEnumerable<ProductionOrder>> GetAllFaildOrdersAsync();

}
