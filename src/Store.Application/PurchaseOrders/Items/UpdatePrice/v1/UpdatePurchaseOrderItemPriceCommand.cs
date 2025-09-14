namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Items.UpdatePrice.v1;

public sealed record UpdatePurchaseOrderItemPriceCommand(
    DefaultIdType PurchaseOrderItemId,
    decimal UnitPrice,
    decimal? DiscountAmount = null
) : IRequest;

