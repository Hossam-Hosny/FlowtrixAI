using FlowtrixAI.Domain.Entities;

namespace FlowtrixAI.Domain.Repositories;

public interface IQualityCheckRepository
{

    Task<List<QualityCheck>> GetAllAsync();
    Task<QualityCheck?> GetByIdAsync(int id);
    Task AddAsync(QualityCheck qc);
    Task UpdateAsync(QualityCheck qc);
    Task DeleteAsync(int id);
    Task<IEnumerable<QualityCheck>> GetLatestAsync(int count);
}
