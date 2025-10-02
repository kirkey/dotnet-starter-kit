// InventoryItem Exceptions

using FSH.Framework.Core.Exceptions;

namespace Accounting.Domain.Exceptions;

public sealed class InventoryItemNotFoundException(DefaultIdType id) : NotFoundException($"inventory item with id {id} not found");
public sealed class InsufficientStockException(DefaultIdType itemId, decimal requested, decimal available) 
    : ForbiddenException($"insufficient stock for item {itemId}. Requested: {requested}, Available: {available}");
public sealed class InvalidInventoryQuantityException() : ForbiddenException("inventory quantity must be non-negative");
public sealed class InvalidInventoryUnitPriceException() : ForbiddenException("inventory unit price cannot be negative");
public sealed class InvalidInventoryReorderLevelException() : ForbiddenException("reorder level cannot be negative");
public sealed class InventoryItemAlreadyInactiveException(DefaultIdType id) : ForbiddenException($"inventory item with id {id} is already inactive");