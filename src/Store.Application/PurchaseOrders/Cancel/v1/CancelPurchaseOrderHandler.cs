using FSH.Framework.Core.Exceptions;
using Store.Domain.Exceptions.PurchaseOrder;

namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Cancel.v1;

/// <summary>
/// Handler for cancelling a purchase order.
/// Validates that the order exists and can be cancelled (not yet received or already cancelled).
/// </summary>
public sealed class CancelPurchaseOrderHandler(
    ILogger<CancelPurchaseOrderHandler> logger,
    [FromKeyedServices("store:purchase-orders")] IRepository<PurchaseOrder> repository)
    : IRequestHandler<CancelPurchaseOrderCommand, CancelPurchaseOrderResponse>
{
    public async Task<CancelPurchaseOrderResponse> Handle(CancelPurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var purchaseOrder = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = purchaseOrder ?? throw new PurchaseOrderNotFoundException(request.Id);

        // Validate business rules for cancellation
        if (purchaseOrder.Status == PurchaseOrderStatus.Cancelled)
            throw new ConflictException($"Purchase order '{request.Id}' is already cancelled");

        if (purchaseOrder.Status == PurchaseOrderStatus.Received)
            throw new ConflictException($"Purchase order '{request.Id}' cannot be cancelled as it has been received");

        // Check if any items have been received
        if (purchaseOrder.Items.Any(item => item.ReceivedQuantity > 0))
            throw new ConflictException($"Purchase order '{request.Id}' cannot be cancelled as some items have been received");

        // Cancel the purchase order
        purchaseOrder.UpdateStatus(PurchaseOrderStatus.Cancelled);
        
        // Add cancellation reason if provided
        if (!string.IsNullOrWhiteSpace(request.CancellationReason))
        {
            var currentNotes = purchaseOrder.Notes ?? string.Empty;
            var cancellationNote = $"[CANCELLED {DateTime.UtcNow:yyyy-MM-dd HH:mm}]: {request.CancellationReason}";
            var updatedNotes = string.IsNullOrWhiteSpace(currentNotes) 
                ? cancellationNote 
                : $"{currentNotes}\n{cancellationNote}";
            
            purchaseOrder.Update(
                purchaseOrder.OrderNumber,
                purchaseOrder.SupplierId,
                purchaseOrder.OrderDate,
                purchaseOrder.ExpectedDeliveryDate,
                purchaseOrder.Status,
                updatedNotes,
                purchaseOrder.DeliveryAddress,
                purchaseOrder.ContactPerson,
                purchaseOrder.ContactPhone,
                purchaseOrder.IsUrgent);
        }

        await repository.UpdateAsync(purchaseOrder, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Purchase order {PurchaseOrderId} cancelled successfully", purchaseOrder.Id);
        return new CancelPurchaseOrderResponse(purchaseOrder.Id, purchaseOrder.Status);
    }
}
