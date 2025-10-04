namespace Store.Domain.Exceptions.InventoryTransferItem;

/// <summary>
/// Exception thrown when an inventory transfer item is not found by ID.
/// </summary>
public sealed class InventoryTransferItemByIdNotFoundException(DefaultIdType id) : NotFoundException($"inventory transfer item with id {id} not found");

/// <summary>
/// Exception thrown when the transfer quantity is invalid.
/// </summary>
public sealed class InvalidTransferQuantityException() : ForbiddenException("transfer quantity must be positive");

/// <summary>
/// Exception thrown when trying to transfer more than available inventory.
/// </summary>
public sealed class InsufficientInventoryForTransferException(decimal availableQuantity, decimal transferQuantity) 
    : ForbiddenException($"insufficient inventory. Available: {availableQuantity}, Requested: {transferQuantity}");

/// <summary>
/// Exception thrown when trying to transfer between the same warehouse.
/// </summary>
public sealed class SameWarehouseTransferException() : ForbiddenException("cannot transfer inventory to the same warehouse");

/// <summary>
/// Exception thrown when trying to modify a completed transfer item.
/// </summary>
public sealed class CannotModifyCompletedTransferItemException(DefaultIdType id) : ForbiddenException($"cannot modify completed inventory transfer item with id {id}");

/// <summary>
/// Exception thrown when trying to add a duplicate item to an inventory transfer.
/// </summary>
public sealed class DuplicateInventoryTransferItemException(DefaultIdType transferId, DefaultIdType itemId) : ConflictException($"item {itemId} already exists in inventory transfer {transferId}");
