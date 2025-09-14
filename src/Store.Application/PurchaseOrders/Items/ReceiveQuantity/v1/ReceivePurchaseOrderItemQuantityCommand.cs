namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Items.ReceiveQuantity.v1;

public sealed record ReceivePurchaseOrderItemQuantityCommand(
    DefaultIdType PurchaseOrderItemId,
    int ReceivedQuantity
) : IRequest;

