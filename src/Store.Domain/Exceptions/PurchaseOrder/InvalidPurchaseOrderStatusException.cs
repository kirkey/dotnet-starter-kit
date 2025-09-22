namespace Store.Domain.Exceptions.PurchaseOrder;

/// <summary>
/// Thrown when an invalid status is used with a PurchaseOrder lifecycle method.
/// </summary>
public sealed class InvalidPurchaseOrderStatusException(string? status)
    : CustomException($"Invalid purchase order status: '{status}'.") {}

