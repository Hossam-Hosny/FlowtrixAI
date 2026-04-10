using FlowtrixAI.Domain.Entities;

namespace FlowtrixAI.Domain.Repositories;

public interface IReportRepository
{

    Task<List<Report>> GetAllAsync();
    Task<Report?> GetByIdAsync(int id);
    Task AddAsync(Report report);
    Task UpdateAsync(Report report);
    Task DeleteAsync(int id);
}
