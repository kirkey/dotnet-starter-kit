namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Update.v1;

/// <summary>
/// Response returned after updating a purchase order.
/// </summary>
/// <param name="Id">The identifier of the updated purchase order.</param>
public record UpdatePurchaseOrderResponse(DefaultIdType Id);
