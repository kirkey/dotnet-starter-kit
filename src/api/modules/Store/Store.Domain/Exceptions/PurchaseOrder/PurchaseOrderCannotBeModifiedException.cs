namespace Store.Domain.Exceptions.PurchaseOrder;

/// <summary>
/// Thrown when a mutation is attempted on a purchase order that cannot be modified
/// due to its current lifecycle state (for example, Approved, Received, or Cancelled).
/// </summary>
public sealed class PurchaseOrderCannotBeModifiedException(DefaultIdType id, string status)
    : ConflictException($"Purchase Order '{id}' cannot be modified in status '{status}'.") {}

