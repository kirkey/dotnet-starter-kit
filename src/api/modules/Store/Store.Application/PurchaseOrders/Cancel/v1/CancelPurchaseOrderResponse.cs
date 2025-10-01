namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Cancel.v1;

/// <summary>
/// Response for CancelPurchaseOrderCommand.
/// Returns the purchase order ID and updated status after cancellation.
/// </summary>
public sealed record CancelPurchaseOrderResponse(DefaultIdType Id, string Status);
