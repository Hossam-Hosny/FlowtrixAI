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
        var order = await _productionOrderRepository.GetByIdAsync(orderId);

        if (order == null)
            return "الطلب غير موجود";

        if (order.Status != OrderSteps.InProgress)
            return $"لا يمكن إكمال الطلب. الحالة الحالية: {order.Status}";

        order.Status = OrderSteps.Completed;
        await _productionOrderRepository.UpdateAsync(order);

        // Inventory Logic: Add finished products to stock
        var product = await _productRepository.GetByIdAsync(order.ProductId);
        if (product != null)
        {
            product.StockQuantity += order.Quantity;
            await _productRepository.UpdateAsync(product);
        }

        return "تم اكتمال الطلب";
    }

    public async Task<ProductionOrderOperationResponse> CreateOrderAsync(int productId, int quantity, int createdBy)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null) return new ProductionOrderOperationResponse { Success = false, Message = "المنتج غير موجود في قاعدة البيانات!" };
        
        // get bom 
        var bom = await _bomRepository.GetByIdAsync(productId);


        // check inventory for each component and quantity in bom
        foreach (var item in bom)
        {
            var inventory = await _inventoryRepository.GetByNameAsync(item.ComponentName);

            var requiredQuantity = item.QuantityRequired * quantity;

            if (inventory == null || inventory.QuantityAvailable < requiredQuantity)
            {
                return new ProductionOrderOperationResponse { Success = false, Message = $"مرفوض: لا يوجد مواد كافية في المخزن للمكون: {item.ComponentName}" };
            }


        }

        // deduct materials 
        foreach (var item in bom)
        {
            var inventory = await _inventoryRepository.GetByNameAsync(item.ComponentName);
            var deductionAmount = item.QuantityRequired * (decimal)quantity;
            inventory.QuantityAvailable -= deductionAmount;
            inventory.TotalUsed += deductionAmount;
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
        return new ProductionOrderOperationResponse { Success = true, Message = "تم قبول الطلب", OrderId = order.Id };
    }

    public async Task<string> DeliverOrderAsync(int orderId)
    {
        var order = _productionOrderRepository.GetByIdAsync(orderId).Result;

        if (order == null)
            return "الطلب غير موجود";

        if(order.Status != OrderSteps.Completed)
            return $"لا يمكن توصيل الطلب. الحالة الحالية: {order.Status}";

        order.Status = OrderSteps.Delivered;
        await _productionOrderRepository.UpdateAsync(order);

        return "تم توصيل الطلب";

    }

    public async Task<string> FailOrderAsync(int orderId, string problemDescription, int userId)
    {
        var order = await _productionOrderRepository.GetByIdAsync(orderId);

        if (order == null)
            return "Order not found";

        order.Status = OrderSteps.Rejected;
        order.ProblemDescription = problemDescription;
        order.ReportedByUserId = userId;

        await _productionOrderRepository.UpdateAsync(order);

        return "Order Rejected";
    }

    public async Task<string> UpdateOrderProgressAsync(int orderId, int stepIndex)
    {
        var order = await _productionOrderRepository.GetByIdAsync(orderId);
        if (order == null) return "Order not found";
        
        order.CurrentStepIndex = stepIndex;
        await _productionOrderRepository.UpdateAsync(order);
        return "Progress Updated";
    }

    public async Task<IEnumerable<ProductionOrdersResponse>> GetAllOrdersAsync()
    {
        var orders = await _productionOrderRepository.GetAllAsync();


        var response = orders.Select(o => {
            var totalSteps = o.Product?.Processes?.Count ?? 0;
            var progress = 0;
            
            if (o.Status == OrderSteps.Completed)
            {
                progress = 100;
            }
            else if (totalSteps > 0)
            {
                progress = (o.CurrentStepIndex * 100) / totalSteps;
            }

            return new ProductionOrdersResponse
            {
                OrderId = o.Id,
                OrderName = o.Product?.Name ?? "Unknown",
                Quantity = o.Quantity,
                OrderdAt = o.CreatedAt,
                Status = o.Status.ToString(),
                Progress = progress,
                ProblemDescription = o.ProblemDescription,
                ReportedByUserId = o.ReportedByUserId,
                ReportedByUserName = o.ReportedBy?.Name
            };
        });

        return response;


    }

    public async Task<int> GetNumberOfAllCompletedOrders()
    {
       var orders = await _productionOrderRepository.GetAllCompletedOrders();
        return orders.Count();
        
    }

    public async Task<int> GetNumberOfAllFaildOrders()
    {
        var orders = await _productionOrderRepository.GetAllFaildOrdersAsync();
        return orders.Count();
    }

    public async Task<string> StartOrderAsync(int orderId)
    {
        var order = await _productionOrderRepository.GetByIdAsync(orderId);

        if (order == null) 
            return "الطلب غير موجود";

        if (order.Status != OrderSteps.Approved)
            return $"لا يمكن بدء الطلب. الحالة الحالية: {order.Status}";

        order.Status = OrderSteps.InProgress;

        await _productionOrderRepository.UpdateAsync(order);
        return "تم بدء الطلب";
    }

    public async Task<ProductionOrdersResponse?> GetOrderByIdAsync(int id)
    {
        var o = await _productionOrderRepository.GetByIdAsync(id);
        if (o == null) return null;

        var totalSteps = o.Product?.Processes?.Count ?? 0;
        var progress = 0;

        if (o.Status == OrderSteps.Completed)
        {
            progress = 100;
        }
        else if (totalSteps > 0)
        {
            progress = (o.CurrentStepIndex * 100) / totalSteps;
        }

        return new ProductionOrdersResponse
        {
            OrderId = o.Id,
            OrderName = o.Product?.Name ?? "Unknown",
            Quantity = o.Quantity,
            OrderdAt = o.CreatedAt,
            Status = o.Status.ToString(),
            Progress = progress,
            ProblemDescription = o.ProblemDescription,
            ReportedByUserId = o.ReportedByUserId,
            ReportedByUserName = o.ReportedBy?.Name
        };
    }

    public async Task<string> CancelOrderAsync(int orderId)
    {
        var order = await _productionOrderRepository.GetByIdAsync(orderId);
        if (order == null) return "الطلب غير موجود";

        if (order.Status != OrderSteps.Approved)
            return "لا يمكن إلغاء الطلب إلا إذا كان في حالة 'جاهزة للبدء'";

        // get bom 
        var bom = await _bomRepository.GetByIdAsync(order.ProductId);

        // Refund materials 
        foreach (var item in bom)
        {
            var inventory = await _inventoryRepository.GetByNameAsync(item.ComponentName);
            if (inventory != null)
            {
                var refundAmount = item.QuantityRequired * (decimal)order.Quantity;
                inventory.QuantityAvailable += refundAmount;
                inventory.TotalUsed -= refundAmount;
                await _inventoryRepository.UpdateAsync(inventory);
            }
        }

        await _productionOrderRepository.DeleteAsync(order);
        return "تم إلغاء الطلب وإرجاع الكميات للمخزن بنجاح";
    }
}
