namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Submit.v1;

/// <summary>
/// Response for SubmitPurchaseOrderCommand.
/// Returns the purchase order ID and updated status after submission.
/// </summary>
public sealed record SubmitPurchaseOrderResponse(DefaultIdType Id, string Status);
