using FSH.Framework.Core.Exceptions;
using Store.Domain.Exceptions.PurchaseOrder;

namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Receive.v1;

/// <summary>
/// Handler for receiving a purchase order delivery.
/// Validates that the order exists and can be received (is in Sent status).
/// </summary>
public sealed class ReceivePurchaseOrderHandler(
    ILogger<ReceivePurchaseOrderHandler> logger,
    [FromKeyedServices("store:purchase-orders")] IRepository<PurchaseOrder> repository)
    : IRequestHandler<ReceivePurchaseOrderCommand, ReceivePurchaseOrderResponse>
{
    public async Task<ReceivePurchaseOrderResponse> Handle(ReceivePurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var purchaseOrder = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = purchaseOrder ?? throw new PurchaseOrderNotFoundException(request.Id);

        // Validate business rules for receiving
        if (purchaseOrder.Status == PurchaseOrderStatus.Received)
            throw new ConflictException($"Purchase order '{request.Id}' has already been received");

        if (purchaseOrder.Status == PurchaseOrderStatus.Cancelled)
            throw new ConflictException($"Purchase order '{request.Id}' cannot be received as it has been cancelled");

        if (purchaseOrder.Status != PurchaseOrderStatus.Sent)
            throw new ConflictException($"Purchase order '{request.Id}' cannot be received. Current status: {purchaseOrder.Status}. Only sent orders can be received.");

        // Set actual delivery date (default to current date if not provided)
        var actualDeliveryDate = request.ActualDeliveryDate ?? DateTime.UtcNow;
        purchaseOrder.UpdateDeliveryDate(actualDeliveryDate);
        
        // Add receipt notes if provided
        if (!string.IsNullOrWhiteSpace(request.ReceiptNotes))
        {
            var currentNotes = purchaseOrder.Notes ?? string.Empty;
            var receiptNote = $"[RECEIVED {actualDeliveryDate:yyyy-MM-dd HH:mm}]: {request.ReceiptNotes}";
            var updatedNotes = string.IsNullOrWhiteSpace(currentNotes) 
                ? receiptNote 
                : $"{currentNotes}\n{receiptNote}";
            
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

        logger.LogInformation("Purchase order {PurchaseOrderId} received successfully on {DeliveryDate}", 
            purchaseOrder.Id, actualDeliveryDate);
        
        return new ReceivePurchaseOrderResponse(purchaseOrder.Id, purchaseOrder.Status, actualDeliveryDate);
    }
}
