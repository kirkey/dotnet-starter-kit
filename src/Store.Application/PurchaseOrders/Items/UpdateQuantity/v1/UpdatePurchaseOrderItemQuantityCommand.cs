namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Items.UpdateQuantity.v1;

public sealed record UpdatePurchaseOrderItemQuantityCommand(
    DefaultIdType PurchaseOrderItemId,
    int Quantity
) : IRequest;

