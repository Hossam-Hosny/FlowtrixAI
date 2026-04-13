using FlowtrixAI.Application.ProductionOrder.Dtos;
using FlowtrixAI.Application.ProductionOrder.Interface;
using FlowtrixAI.Domain.Constants;
using FlowtrixAI.Domain.Repositories;

namespace FlowtrixAI.Application.ProductionOrder.Services;

internal class ProductionOrderService(IBomRepository _bomRepository 
    ,IInventoryRepository _inventoryRepository , IProductionOrderRepository _productionOrderRepository
    ,IProductRepository _productRepository) 
    : IProductionOrderService
{
    public async Task<string> CompleteOrderAsync(int orderId)
    {
        var order =  _productionOrderRepository.GetByIdAsync(orderId).Result;

        if (order == null)
            return "Order not found";

        if (order.Status != OrderSteps.InProgress)
            return "Order cannot be completed. Current status: " + order.Status;

        order.Status = OrderSteps.Completed;
        await _productionOrderRepository.UpdateAsync(order);

        return "Order Completed";

    }

    public async Task<string> CreateOrderAsync(int productId, int quantity, int createdBy)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null) return "Order does not exist in Db!";
        
        // get bom 
        var bom = await _bomRepository.GetByIdAsync(productId);


        // check inventory for each component and quantity in bom
        foreach (var item in bom)
        {
            var inventory = await _inventoryRepository.GetByNameAsync(item.ComponentName);

            var requiredQuantity = item.QuantityRequired * quantity;

            if (inventory == null || inventory.QuantityAvailable < requiredQuantity)
            {
                return $"Rejected : Not enough Materials in  inventory for component: {item.ComponentName}";
            }


        }

        // deduct materials 
        foreach (var item in bom)
        {

            var inventory = await _inventoryRepository.GetByNameAsync(item.ComponentName);

            inventory.QuantityAvailable -= item.QuantityRequired * quantity;
            await _inventoryRepository.UpdateAsync(inventory);
        }

        // create production order
        var order = new Domain.Entities.ProductionOrder
        {
            ProductId = productId,
            Quantity = quantity,

            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy,
            Status = OrderSteps.Approved
            

        };
        await _productionOrderRepository.AddAsync(order);
        return "Order Approved";

    }

    public async Task<string> DeliverOrderAsync(int orderId)
    {
        var order = _productionOrderRepository.GetByIdAsync(orderId).Result;

        if (order == null)
            return "Order not found";

        if(order.Status != OrderSteps.Completed)
            return "Order cannot be delivered. Current status: " + order.Status;

        order.Status = OrderSteps.Delivered;
        await _productionOrderRepository.UpdateAsync(order);

        return "Order Delivered";

    }

    public async Task<string> FailOrderAsync(int orderId)
    {
        var order = _productionOrderRepository.GetByIdAsync(orderId).Result ;

        if (order == null)
            return "Order not found";

        order.Status = OrderSteps.Rejected;
        await _productionOrderRepository.UpdateAsync(order);

        return "Order Rejected";

    }

    public async Task<IEnumerable<ProductionOrdersResponse>> GetAllOrdersAsync()
    {
        var orders = await _productionOrderRepository.GetAllAsync();


        var response = orders.Select(o => new ProductionOrdersResponse
        {
            OrderId = o.Id,
            OrderName = o.Product.Name,
            Quantity = o.Quantity,
            OrderdAt = o.CreatedAt,
            Status = o.Status.ToString()
        });

        return response;


    }

    public async Task<int> GetNumberOfAllCompletedOrders()
    {
       var orders = await _productionOrderRepository.GetAllCompletedOrders();
        return orders.Count();
        
    }

    public async Task<string> StartOrderAsync(int orderId)
    {
        var order = await _productionOrderRepository.GetByIdAsync(orderId);

        if (order == null) 
            return "Order not found";

        if (order.Status != OrderSteps.Approved)
            return "Order cannot be started. Current status: " + order.Status;

        order.Status = OrderSteps.InProgress;

        await _productionOrderRepository.UpdateAsync(order);
        return "Order Started";
    }
}
