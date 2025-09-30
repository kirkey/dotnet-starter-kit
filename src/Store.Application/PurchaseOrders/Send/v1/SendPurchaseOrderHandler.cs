using FSH.Framework.Core.Exceptions;
using Store.Domain.Exceptions.PurchaseOrder;

namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Send.v1;

/// <summary>
/// Handler for sending an approved purchase order to the supplier.
/// Validates that the order exists and can be sent (is in Approved status).
/// </summary>
public sealed class SendPurchaseOrderHandler(
    ILogger<SendPurchaseOrderHandler> logger,
    [FromKeyedServices("store:purchase-orders")] IRepository<PurchaseOrder> repository)
    : IRequestHandler<SendPurchaseOrderCommand, SendPurchaseOrderResponse>
{
    public async Task<SendPurchaseOrderResponse> Handle(SendPurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var purchaseOrder = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = purchaseOrder ?? throw new PurchaseOrderNotFoundException(request.Id);

        // Validate business rules for sending
        if (purchaseOrder.Status != PurchaseOrderStatus.Approved)
            throw new ConflictException($"Purchase order '{request.Id}' cannot be sent. Current status: {purchaseOrder.Status}. Only approved orders can be sent.");

        if (!purchaseOrder.Items.Any())
            throw new ConflictException($"Purchase order '{request.Id}' cannot be sent without items");

        // Send the purchase order
        purchaseOrder.UpdateStatus(PurchaseOrderStatus.Sent);
        
        // Add delivery instructions if provided
        if (!string.IsNullOrWhiteSpace(request.DeliveryInstructions))
        {
            var currentNotes = purchaseOrder.Notes ?? string.Empty;
            var deliveryNote = $"[SENT {DateTime.UtcNow:yyyy-MM-dd HH:mm}] Delivery Instructions: {request.DeliveryInstructions}";
            var updatedNotes = string.IsNullOrWhiteSpace(currentNotes) 
                ? deliveryNote 
                : $"{currentNotes}\n{deliveryNote}";
            
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

        logger.LogInformation("Purchase order {PurchaseOrderId} sent to supplier successfully", purchaseOrder.Id);
        return new SendPurchaseOrderResponse(purchaseOrder.Id, purchaseOrder.Status, DateTime.UtcNow);
    }
}
