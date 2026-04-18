using FlowtrixAI.Domain.Entities;

namespace FlowtrixAI.Domain.Repositories;

public interface IProductionRecordRepository
{
    Task<IEnumerable<ProductionRecord>> GetAllAsync();
    Task<ProductionRecord?> GetByIdAsync(int id);
    Task AddAsync(ProductionRecord record);
    Task UpdateAsync(ProductionRecord record);
    Task DeleteAsync(int id);
    Task<IEnumerable<ProductionRecord>> GetLatestAsync(int count);
}
