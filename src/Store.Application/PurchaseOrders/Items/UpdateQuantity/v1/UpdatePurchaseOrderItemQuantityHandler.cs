using Store.Domain.Exceptions.PurchaseOrder;

namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Items.UpdateQuantity.v1;

public sealed class UpdatePurchaseOrderItemQuantityHandler(
    ILogger<UpdatePurchaseOrderItemQuantityHandler> logger,
    [FromKeyedServices("store:purchase-orders")] IRepository<PurchaseOrder> repository)
    : IRequestHandler<UpdatePurchaseOrderItemQuantityCommand>
{
    public async Task Handle(UpdatePurchaseOrderItemQuantityCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var po = await repository.FirstOrDefaultAsync(new Specs.PurchaseOrderByItemIdSpec(request.PurchaseOrderItemId), cancellationToken).ConfigureAwait(false);
        _ = po ?? throw new PurchaseOrderItemNotFoundException(request.PurchaseOrderItemId);

        // Delegate to aggregate
        var updated = po.UpdateItemQuantity(request.PurchaseOrderItemId, request.Quantity);
        await repository.UpdateAsync(updated, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Updated item quantity for purchase order {PurchaseOrderId}", po.Id);
    }
}
