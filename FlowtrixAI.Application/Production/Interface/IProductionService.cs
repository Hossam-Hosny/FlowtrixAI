using FlowtrixAI.Domain.Entities;

namespace FlowtrixAI.Application.Production.Interface;

public interface IProductionService
{
    Task CreateProductionAsync(int productId, decimal quantity,int processId,int engineerId);
    Task<IEnumerable<ProductionRecord>> GetAllAsync();
}

