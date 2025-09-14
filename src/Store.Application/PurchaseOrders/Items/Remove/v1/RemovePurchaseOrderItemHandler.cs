using Store.Domain.Exceptions.PurchaseOrder;

namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Items.Remove.v1;

public sealed class RemovePurchaseOrderItemHandler(
    ILogger<RemovePurchaseOrderItemHandler> logger,
    [FromKeyedServices("store:purchase-orders")] IRepository<PurchaseOrder> repository)
    : IRequestHandler<RemovePurchaseOrderItemCommand>
{
    public async Task Handle(RemovePurchaseOrderItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var po = await repository.GetByIdAsync(request.PurchaseOrderId, cancellationToken).ConfigureAwait(false);
        _ = po ?? throw new PurchaseOrderNotFoundException(request.PurchaseOrderId);

        var updated = po.RemoveItem(request.GroceryItemId);

        await repository.UpdateAsync(updated, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Removed item from purchase order {PurchaseOrderId}", po.Id);
    }
}

