namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Send.v1;

/// <summary>
/// Response for SendPurchaseOrderCommand.
/// Returns the purchase order ID, updated status, and timestamp when sent.
/// </summary>
public sealed record SendPurchaseOrderResponse(DefaultIdType Id, string Status, DateTime SentDate);
