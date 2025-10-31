namespace Accounting.Application.PurchaseOrders.Create.v1;

/// <summary>
/// Response containing the created purchase order ID.
/// </summary>
public record PurchaseOrderCreateResponse(DefaultIdType Id);

