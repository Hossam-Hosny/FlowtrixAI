using FlowtrixAI.Application.Export.Dtos;

namespace FlowtrixAI.Application.Export.Interface
{
    public interface IExportService
    {
        Task<IEnumerable<ExportDto>> GetAllExportsAsync();
        Task<ExportDto?> GetExportByIdAsync(int id);
        Task<bool> CreateExportAsync(CreateExportDto createExportDto);
        Task<bool> UpdateExportStatusAsync(int id, string status);
        Task<bool> DeleteExportAsync(int id);
    }
}
