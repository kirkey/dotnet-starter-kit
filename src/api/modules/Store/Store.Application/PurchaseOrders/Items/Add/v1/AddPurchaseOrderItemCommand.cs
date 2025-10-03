namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Items.Add.v1;

public sealed record AddPurchaseOrderItemCommand(
    DefaultIdType PurchaseOrderId,
    DefaultIdType ItemId,
    int Quantity,
    decimal UnitPrice,
    decimal? Discount = null
) : IRequest<AddPurchaseOrderItemResponse>;

