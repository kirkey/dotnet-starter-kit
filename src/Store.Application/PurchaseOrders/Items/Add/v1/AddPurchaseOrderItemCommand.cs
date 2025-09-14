namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Items.Add.v1;

public sealed record AddPurchaseOrderItemCommand(
    DefaultIdType PurchaseOrderId,
    DefaultIdType GroceryItemId,
    int Quantity,
    decimal UnitPrice,
    decimal? Discount = null
) : IRequest<AddPurchaseOrderItemResponse>;

