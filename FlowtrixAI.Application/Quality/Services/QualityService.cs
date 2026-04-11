using FlowtrixAI.Application.Quality.Interface;
using FlowtrixAI.Domain.Entities;
using FlowtrixAI.Domain.Exceptions;
using FlowtrixAI.Domain.Repositories;

namespace FlowtrixAI.Application.Quality.Services;

internal class QualityService(IQualityCheckRepository _qualityCheckRepository ,IProductionRecordRepository _productionRecordRepository)
    : IQualityService
{
    /// <summary>
    /// Records a quality check for a given production record. It calculates the defect ratio and adds notes if the ratio exceeds 10%. The quality check is then saved to the database.
    /// </summary>
    /// <param name="productionRecordId"></param>
    /// <param name="defectsCount"></param>
    /// <param name="checkedById"></param>
    /// <param name="notes"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task AddQualityCheckAsync(int productionRecordId, int defectsCount, int checkedById, string notes)
    {
        // Get Prodeuction Record
        var productionRecord = await _productionRecordRepository.GetByIdAsync(productionRecordId);

        if (productionRecord == null)
            throw new NotFoundException($"Production record with the specified ID {productionRecordId} is not found.");

        // Calculate defect ratio
        decimal defectRatio = 0;
        if (productionRecord.QuantityProduced > 0)
            defectRatio = (decimal)defectsCount / productionRecord.QuantityProduced;


        // Business rule: If defect ratio is greater than 10% we consider it a critical issue
        if (defectRatio > 0.1m)
        {
            notes += "⚠️ Critical issue: Defect ratio exceeds 10%. Immediate attention required.";

        }

        // Save quality check
        var qualityCheck = new QualityCheck
        {
            ProductionRecordId = productionRecordId,
            DecfectCount = defectsCount,
            CheckedById = checkedById,
            Notes = notes,
            CheckAt = DateTime.UtcNow
        };

        await _qualityCheckRepository.AddAsync(qualityCheck);
    }

    public async Task<IEnumerable<QualityCheck>> GetAllAsync()=> await _qualityCheckRepository.GetAllAsync();
   
}
