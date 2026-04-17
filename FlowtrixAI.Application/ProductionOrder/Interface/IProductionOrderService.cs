using FlowtrixAI.Application.ProductionOrder.Dtos;
using FlowtrixAI.Domain.Entities;

namespace FlowtrixAI.Application.ProductionOrder.Interface;

public interface IProductionOrderService
{
    Task<ProductionOrderOperationResponse> CreateOrderAsync(int productId, int quantity, int createdBy);
    Task<string> StartOrderAsync(int orderId);
    Task<string> CompleteOrderAsync(int orderId);
    Task<string> FailOrderAsync(int orderId, string problemDescription, int userId);
    Task<string> DeliverOrderAsync(int orderId);
    Task<string> UpdateOrderProgressAsync(int orderId, int stepIndex);
    Task<int> GetNumberOfAllCompletedOrders();
    Task<int> GetNumberOfAllFaildOrders();
    Task<IEnumerable<ProductionOrdersResponse>> GetAllOrdersAsync();
    Task<ProductionOrdersResponse?> GetOrderByIdAsync(int id);
    Task<string> CancelOrderAsync(int orderId);
}
