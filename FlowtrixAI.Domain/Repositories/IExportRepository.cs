using FlowtrixAI.Domain.Entities;

namespace FlowtrixAI.Domain.Repositories
{
    public interface IExportRepository
    {
        Task<IEnumerable<Export>> GetAllAsync();
        Task<Export?> GetByIdAsync(int id);
        Task<Export> AddAsync(Export export);
        Task UpdateAsync(Export export);
        Task DeleteAsync(int id);
        Task<IEnumerable<Export>> GetLatestAsync(int count);
        Task<IEnumerable<Export>> GetByDateRangeAsync(DateTime start, DateTime end);
    }
}
