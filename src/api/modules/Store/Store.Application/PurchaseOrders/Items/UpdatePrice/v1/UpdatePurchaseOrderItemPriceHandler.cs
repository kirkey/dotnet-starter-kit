using Store.Domain.Exceptions.PurchaseOrder;

namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Items.UpdatePrice.v1;

public sealed class UpdatePurchaseOrderItemPriceHandler(
    ILogger<UpdatePurchaseOrderItemPriceHandler> logger,
    [FromKeyedServices("store:purchase-orders")] IRepository<PurchaseOrder> repository)
    : IRequestHandler<UpdatePurchaseOrderItemPriceCommand>
{
    public async Task Handle(UpdatePurchaseOrderItemPriceCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var po = await repository.FirstOrDefaultAsync(new Specs.PurchaseOrderByItemIdSpec(request.PurchaseOrderItemId), cancellationToken).ConfigureAwait(false);
        _ = po ?? throw new PurchaseOrderItemNotFoundException(request.PurchaseOrderItemId);

        // Delegate to aggregate (which will validate item existence and recalc totals)
        var updated = po.UpdateItemPrice(request.PurchaseOrderItemId, request.UnitPrice, request.DiscountAmount);
        await repository.UpdateAsync(updated, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Updated item price for purchase order {PurchaseOrderId}", po.Id);
    }
}
