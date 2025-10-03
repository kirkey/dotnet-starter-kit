namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Items.Remove.v1;

public sealed record RemovePurchaseOrderItemCommand(
    DefaultIdType PurchaseOrderId,
    DefaultIdType ItemId
) : IRequest;

