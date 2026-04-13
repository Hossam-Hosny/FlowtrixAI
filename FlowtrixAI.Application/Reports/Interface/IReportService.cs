
using FlowtrixAI.Application.Reports.Dtos;
using FlowtrixAI.Domain.Entities;

namespace FlowtrixAI.Application.Reports.Interface;

public interface IReportService
{
    Task GenerateReportAsync(int generatedById);
    Task<IEnumerable<Report>> GetAllAsync();
    Task<ReportDto> GetSystemReportAsync();

}
