namespace Store.Domain.Exceptions.ShipmentItem;

/// <summary>
/// Exception thrown when a shipment item is not found by ID.
/// </summary>
public sealed class ShipmentItemByIdNotFoundException(DefaultIdType id) : NotFoundException($"shipment item with id {id} not found");

/// <summary>
/// Exception thrown when shipment item quantity is invalid.
/// </summary>
public sealed class InvalidShipmentItemQuantityException() : ForbiddenException("shipment item quantity must be positive");

/// <summary>
/// Exception thrown when trying to add a duplicate item to a shipment.
/// </summary>
public sealed class DuplicateShipmentItemException(DefaultIdType shipmentId, DefaultIdType itemId) : ConflictException($"item {itemId} already exists in shipment {shipmentId}");

/// <summary>
/// Exception thrown when trying to modify a dispatched shipment item.
/// </summary>
public sealed class CannotModifyDispatchedShipmentItemException(DefaultIdType id) : ForbiddenException($"cannot modify dispatched shipment item with id {id}");

/// <summary>
/// Exception thrown when trying to ship more than available inventory.
/// </summary>
public sealed class InsufficientInventoryForShipmentException(decimal availableQuantity, decimal requestedQuantity) 
    : ForbiddenException($"insufficient inventory. Available: {availableQuantity}, Requested: {requestedQuantity}");

/// <summary>
/// Exception thrown when shipment item unit price is invalid.
/// </summary>
public sealed class InvalidShipmentItemPriceException() : ForbiddenException("shipment item unit price cannot be negative");
