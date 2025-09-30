namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Approve.v1;

/// <summary>
/// Response for ApprovePurchaseOrderCommand.
/// Returns the purchase order ID and updated status after approval.
/// </summary>
public sealed record ApprovePurchaseOrderResponse(DefaultIdType Id, string Status);
