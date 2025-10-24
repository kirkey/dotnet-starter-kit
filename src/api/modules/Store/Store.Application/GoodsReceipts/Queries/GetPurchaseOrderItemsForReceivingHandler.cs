using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Specs;
using Store.Domain.Exceptions.PurchaseOrder;

namespace FSH.Starter.WebApi.Store.Application.GoodsReceipts.Queries;

/// <summary>
/// Handler to get purchase order items available for receiving.
/// Shows ordered vs received quantities to support partial receiving.
/// </summary>
public sealed class GetPurchaseOrderItemsForReceivingHandler(
    [FromKeyedServices("store:purchase-orders")] IRepository<PurchaseOrder> purchaseOrderRepository)
    : IRequestHandler<GetPurchaseOrderItemsForReceivingQuery, GetPurchaseOrderItemsForReceivingResponse>
{
    public async Task<GetPurchaseOrderItemsForReceivingResponse> Handle(
        GetPurchaseOrderItemsForReceivingQuery request, 
        CancellationToken cancellationToken)
    {
        // Load purchase order with items
        var spec = new PurchaseOrderByIdWithItemsSpec(request.PurchaseOrderId);
        var purchaseOrder = await purchaseOrderRepository.FirstOrDefaultAsync(spec, cancellationToken)
            ?? throw new PurchaseOrderNotFoundException(request.PurchaseOrderId);

        // Map to response
        var response = new GetPurchaseOrderItemsForReceivingResponse
        {
            PurchaseOrderId = purchaseOrder.Id,
            OrderNumber = purchaseOrder.OrderNumber,
            Status = purchaseOrder.Status,
            Items = purchaseOrder.Items.Select(item => new PurchaseOrderItemForReceiving
            {
                PurchaseOrderItemId = item.Id,
                ItemId = item.ItemId,
                ItemName = item.Item.Name,
                ItemSku = item.Item.Sku,
                OrderedQuantity = item.Quantity,
                ReceivedQuantity = item.ReceivedQuantity,
                RemainingQuantity = item.Quantity - item.ReceivedQuantity,
                UnitPrice = item.UnitPrice,
                IsFullyReceived = item.ReceivedQuantity >= item.Quantity
            })
            .OrderBy(i => i.IsFullyReceived) // Show pending items first
            .ThenBy(i => i.ItemName)
            .ToList()
        };

        return response;
    }
}

