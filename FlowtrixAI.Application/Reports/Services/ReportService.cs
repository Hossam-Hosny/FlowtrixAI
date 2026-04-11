using FlowtrixAI.Application.Reports.Interface;
using FlowtrixAI.Domain.Entities;
using FlowtrixAI.Domain.Repositories;

namespace FlowtrixAI.Application.Reports.Services;

internal class ReportService(IReportRepository _reportRepository
    ,IProductionRecordRepository _productionRecordRepository
    ,IQualityCheckRepository _qualityCheckRepository 
    , IInventoryRepository _inventoryRepository)

    : IReportService
{
    public async Task GenerateReportAsync(int generatedById)
    {
        // Get Data from repositories
        var productionRecords = await _productionRecordRepository.GetAllAsync();
        var qualityChecks = await _qualityCheckRepository.GetAllAsync();
        var inventory = await _inventoryRepository.GetAllAsync();

        // Process data to create report content
        var totalProduced = productionRecords.Sum(x=>x.QuantityProduced);
        var totalDefects = qualityChecks.Sum(x=>x.DecfectCount);

        decimal defectRate = 0;
        if (totalProduced > 0)
          defectRate = (decimal)totalDefects / totalProduced ;

        var lowStockItems = inventory.Where(x => x.QuantityAvailable < 10)
                                        .Select(x => x.ComponentName)
                                        .ToList();


        // Build report content
        var reportContent = new
        {
            totalProduced = totalProduced,
            totalDefects=totalDefects,
            defectRate=defectRate,
            lowStockItems= lowStockItems,
            GeneratedAt = DateTime.UtcNow
        };

        var jsonData = System.Text.Json.JsonSerializer.Serialize(reportContent);

        // Save report to database
        var report = new Report
        {
           Title = "System Generated Report",
           Type = "Manual",
           GeneratedAt = DateTime.UtcNow,
           GeneratedById = generatedById,
              Data = jsonData
        };

        await _reportRepository.AddAsync(report);

    }

    public async Task<IEnumerable<Report>> GetAllAsync()=> await _reportRepository.GetAllAsync();
    
}
