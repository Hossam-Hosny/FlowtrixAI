using FlowtrixAI.Domain.Entities;

namespace FlowtrixAI.Domain.Repositories;

public interface IBomRepository
{
    Task<IEnumerable<BillOfMaterial>> GetAllAsync();
    Task<List<BillOfMaterial>?> GetByIdAsync(int id);
    Task AddAsync(BillOfMaterial bom);
    Task UpdateAsync(BillOfMaterial bom);
    Task DeleteAsync(int id);

}
