namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Receive.v1;

/// <summary>
/// Response for ReceivePurchaseOrderCommand.
/// Returns the purchase order ID, updated status, and actual delivery date.
/// </summary>
public sealed record ReceivePurchaseOrderResponse(DefaultIdType Id, string Status, DateTime ActualDeliveryDate);
