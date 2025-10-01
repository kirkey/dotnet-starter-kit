using FSH.Framework.Core.Exceptions;
using Store.Domain.Exceptions.PurchaseOrder;

namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Approve.v1;

/// <summary>
/// Handler for approving a submitted purchase order.
/// Validates that the order exists and can be approved (is in Submitted status).
/// </summary>
public sealed class ApprovePurchaseOrderHandler(
    ILogger<ApprovePurchaseOrderHandler> logger,
    [FromKeyedServices("store:purchase-orders")] IRepository<PurchaseOrder> repository)
    : IRequestHandler<ApprovePurchaseOrderCommand, ApprovePurchaseOrderResponse>
{
    public async Task<ApprovePurchaseOrderResponse> Handle(ApprovePurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var purchaseOrder = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = purchaseOrder ?? throw new PurchaseOrderNotFoundException(request.Id);

        // Validate business rules for approval
        if (purchaseOrder.Status != PurchaseOrderStatus.Submitted)
            throw new ConflictException($"Purchase order '{request.Id}' cannot be approved. Current status: {purchaseOrder.Status}");

        // Approve the purchase order
        purchaseOrder.UpdateStatus(PurchaseOrderStatus.Approved);
        
        // Add approval notes if provided
        if (!string.IsNullOrWhiteSpace(request.ApprovalNotes))
        {
            var currentNotes = purchaseOrder.Notes ?? string.Empty;
            var approvalNote = $"[APPROVED {DateTime.UtcNow:yyyy-MM-dd HH:mm}]: {request.ApprovalNotes}";
            var updatedNotes = string.IsNullOrWhiteSpace(currentNotes) 
                ? approvalNote 
                : $"{currentNotes}\n{approvalNote}";
            
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

        logger.LogInformation("Purchase order {PurchaseOrderId} approved successfully", purchaseOrder.Id);
        return new ApprovePurchaseOrderResponse(purchaseOrder.Id, purchaseOrder.Status);
    }
}
