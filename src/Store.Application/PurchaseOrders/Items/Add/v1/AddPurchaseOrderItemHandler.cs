using Store.Domain.Exceptions.PurchaseOrder;

namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Items.Add.v1;

public sealed class AddPurchaseOrderItemHandler(
    ILogger<AddPurchaseOrderItemHandler> logger,
    [FromKeyedServices("store:purchase-orders")] IRepository<PurchaseOrder> repository)
    : IRequestHandler<AddPurchaseOrderItemCommand, AddPurchaseOrderItemResponse>
{
    public async Task<AddPurchaseOrderItemResponse> Handle(AddPurchaseOrderItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var po = await repository.GetByIdAsync(request.PurchaseOrderId, cancellationToken).ConfigureAwait(false);
        _ = po ?? throw new PurchaseOrderNotFoundException(request.PurchaseOrderId);

        // Add item to purchase order (aggregate will handle existing item merge)
        var updated = po.AddItem(request.GroceryItemId, request.Quantity, request.UnitPrice, request.Discount);

        await repository.UpdateAsync(updated, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Added item to purchase order {PurchaseOrderId}", po.Id);
        return new AddPurchaseOrderItemResponse(po.Id);
    }
}

