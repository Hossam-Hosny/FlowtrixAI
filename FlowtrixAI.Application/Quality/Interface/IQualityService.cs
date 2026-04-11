using FlowtrixAI.Domain.Entities;

namespace FlowtrixAI.Application.Quality.Interface;

public interface IQualityService
{
    Task AddQualityCheckAsync(int productionRecordId, int defectsCount, int checkedById,string notes);
    Task<IEnumerable<QualityCheck>> GetAllAsync();
}
