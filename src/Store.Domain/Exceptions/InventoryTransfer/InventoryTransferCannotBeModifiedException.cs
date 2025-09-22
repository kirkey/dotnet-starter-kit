namespace Store.Domain.Exceptions.InventoryTransfer;

/// <summary>
/// Exception thrown when attempting to modify an inventory transfer that cannot be modified
/// due to its current lifecycle state (for example: Completed or Cancelled).
/// </summary>
public sealed class InventoryTransferCannotBeModifiedException(DefaultIdType id)
    : ForbiddenException($"Inventory transfer with id '{id}' cannot be modified in its current state.") {}

