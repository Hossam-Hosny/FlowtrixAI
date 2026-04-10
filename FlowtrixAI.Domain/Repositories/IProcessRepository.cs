using FlowtrixAI.Domain.Entities;

namespace FlowtrixAI.Domain.Repositories;

public interface IProcessRepository
{
    Task<IEnumerable<Process>> GetAllAsync();
    Task<Process?> GetByIdAsync(int id);
    Task AddAsync(Process process);
    Task UpdateAsync(Process process);
    Task DeleteAsync(int id);
}
