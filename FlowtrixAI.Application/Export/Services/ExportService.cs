using FlowtrixAI.Application.Export.Dtos;
using FlowtrixAI.Application.Export.Interface;
using FlowtrixAI.Domain.Entities;
using FlowtrixAI.Domain.Repositories;

namespace FlowtrixAI.Application.Export.Services
{
    public class ExportService : IExportService
    {
        private readonly IExportRepository _exportRepository;
        private readonly IProductRepository _productRepository;

        public ExportService(IExportRepository exportRepository, IProductRepository productRepository)
        {
            _exportRepository = exportRepository;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ExportDto>> GetAllExportsAsync()
        {
            var exports = await _exportRepository.GetAllAsync();
            return exports.Select(e => new ExportDto
            {
                Id = e.Id,
                OrderNumber = e.OrderNumber,
                OrderDate = e.OrderDate,
                ProductId = e.ProductId,
                ProductName = e.Product?.Name ?? "N/A",
                Quantity = e.Quantity,
                Unit = e.Unit,
                UnitPrice = e.UnitPrice,
                TotalAmount = e.TotalAmount,
                BuyerName = e.BuyerName,
                DestinationCountry = e.DestinationCountry,
                Status = e.Status.ToString()
            });
        }

        public async Task<ExportDto?> GetExportByIdAsync(int id)
        {
            var e = await _exportRepository.GetByIdAsync(id);
            if (e == null) return null;

            return new ExportDto
            {
                Id = e.Id,
                OrderNumber = e.OrderNumber,
                OrderDate = e.OrderDate,
                ProductId = e.ProductId,
                ProductName = e.Product?.Name ?? "N/A",
                Quantity = e.Quantity,
                Unit = e.Unit,
                UnitPrice = e.UnitPrice,
                TotalAmount = e.TotalAmount,
                BuyerName = e.BuyerName,
                DestinationCountry = e.DestinationCountry,
                Status = e.Status.ToString()
            };
        }

        public async Task<bool> CreateExportAsync(CreateExportDto createExportDto)
        {
            var export = new FlowtrixAI.Domain.Entities.Export
            {
                OrderNumber = createExportDto.OrderNumber,
                OrderDate = DateTime.Now,
                ProductId = createExportDto.ProductId,
                Quantity = createExportDto.Quantity,
                Unit = createExportDto.Unit,
                UnitPrice = createExportDto.UnitPrice,
                BuyerName = createExportDto.BuyerName,
                DestinationCountry = createExportDto.DestinationCountry,
                Status = Enum.TryParse<ExportStatus>(createExportDto.Status, true, out var initialStatus) ? initialStatus : ExportStatus.Pending
            };

            // Inventory Logic: Decrease Product Stock
            var product = await _productRepository.GetByIdAsync(createExportDto.ProductId);
            if (product != null)
            {
                product.StockQuantity -= createExportDto.Quantity;
                await _productRepository.UpdateAsync(product);
            }

            var result = await _exportRepository.AddAsync(export);
            return result != null;
        }

        public async Task<bool> UpdateExportStatusAsync(int id, string status)
        {
            var export = await _exportRepository.GetByIdAsync(id);
            if (export == null) return false;

            if (Enum.TryParse<ExportStatus>(status, true, out var exportStatus))
            {
                export.Status = exportStatus;
                await _exportRepository.UpdateAsync(export);
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteExportAsync(int id)
        {
            var export = await _exportRepository.GetByIdAsync(id);
            if (export == null) return false;

            // Inventory Logic: Return stock if export is deleted/canceled
            var product = await _productRepository.GetByIdAsync(export.ProductId);
            if (product != null)
            {
                product.StockQuantity += export.Quantity;
                await _productRepository.UpdateAsync(product);
            }

            await _exportRepository.DeleteAsync(id);
            return true;
        }
    }
}
