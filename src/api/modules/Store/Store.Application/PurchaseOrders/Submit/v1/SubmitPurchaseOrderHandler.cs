using FSH.Framework.Core.Exceptions;
using Store.Domain.Exceptions.PurchaseOrder;

namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Submit.v1;

/// <summary>
/// Handler for submitting a purchase order.
/// Validates that the order exists, has items, and can be submitted (is in Draft status).
/// </summary>
public sealed class SubmitPurchaseOrderHandler(
    ILogger<SubmitPurchaseOrderHandler> logger,
    [FromKeyedServices("store:purchase-orders")] IRepository<PurchaseOrder> repository)
    : IRequestHandler<SubmitPurchaseOrderCommand, SubmitPurchaseOrderResponse>
{
    public async Task<SubmitPurchaseOrderResponse> Handle(SubmitPurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var purchaseOrder = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = purchaseOrder ?? throw new PurchaseOrderNotFoundException(request.Id);

        // Validate business rules for submission
        if (purchaseOrder.Status != PurchaseOrderStatus.Draft)
            throw new ConflictException($"Purchase order '{request.Id}' cannot be submitted. Current status: {purchaseOrder.Status}");

        if (purchaseOrder.Items.Count == 0)
            throw new ConflictException($"Purchase order '{request.Id}' cannot be submitted without items");

        if (purchaseOrder.Items.All(item => item.Quantity <= 0))
            throw new ConflictException($"Purchase order '{request.Id}' cannot be submitted with zero quantities");

        // Submit the purchase order
        purchaseOrder.UpdateStatus(PurchaseOrderStatus.Submitted);
        await repository.UpdateAsync(purchaseOrder, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Purchase order {PurchaseOrderId} submitted successfully", purchaseOrder.Id);
        return new SubmitPurchaseOrderResponse(purchaseOrder.Id, purchaseOrder.Status);
    }
}
