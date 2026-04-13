using FlowtrixAI.Domain.Entities;

namespace FlowtrixAI.Application.ProductionOrder.Interface;

public interface IProductionOrderService
{
    Task<string> CreateOrderAsync(int productId, int quantity, int createdBy);
    Task<string> StartOrderAsync(int orderId);

    Task<string> CompleteOrderAsync(int orderId);

    Task<string> FailOrderAsync(int orderId);
    Task<string> DeliverOrderAsync(int orderId);
    Task<int> GetAllCompletedOrders();
    
}
