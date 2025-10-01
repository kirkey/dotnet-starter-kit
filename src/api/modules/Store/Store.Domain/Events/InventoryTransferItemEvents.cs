namespace Store.Domain.Events;

/// <summary>
/// Event raised when an inventory transfer item is removed.
/// </summary>
public record InventoryTransferItemRemoved(
    DefaultIdType Id,
    DefaultIdType InventoryTransferId,
    DefaultIdType GroceryItemId,
    decimal TransferQuantity) : DomainEvent;

/// <summary>
/// Event raised when an inventory transfer item quantity is adjusted.
/// </summary>
public record InventoryTransferItemQuantityAdjusted(
    DefaultIdType Id,
    DefaultIdType InventoryTransferId,
    DefaultIdType GroceryItemId,
    decimal OldQuantity,
    decimal NewQuantity) : DomainEvent;
