namespace Store.Domain.Exceptions.GoodsReceiptItem;

/// <summary>
/// Exception thrown when ID does not find a goods receipt item.
/// </summary>
public sealed class GoodsReceiptItemByIdNotFoundException(DefaultIdType id) : NotFoundException($"goods receipt item with id {id} not found");

/// <summary>
/// Exception thrown when received quantity exceeds ordered quantity.
/// </summary>
public sealed class ReceivedQuantityExceedsOrderedException(decimal orderedQuantity, decimal receivedQuantity) 
    : ForbiddenException($"received quantity {receivedQuantity} exceeds ordered quantity {orderedQuantity}");

/// <summary>
/// Exception thrown when goods receipt item quantity is invalid.
/// </summary>
public sealed class InvalidGoodsReceiptItemQuantityException() : ForbiddenException("goods receipt item quantity cannot be negative");

/// <summary>
/// Exception thrown when goods receipt item unit cost is invalid.
/// </summary>
public sealed class InvalidGoodsReceiptItemCostException() : ForbiddenException("goods receipt item unit cost cannot be negative");

/// <summary>
/// Exception thrown when trying to modify a goods receipt item after processing.
/// </summary>
public sealed class CannotModifyProcessedGoodsReceiptItemException(DefaultIdType id) : ForbiddenException($"cannot modify processed goods receipt item with id {id}");

/// <summary>
/// Exception thrown when trying to add a duplicate item to a goods receipt.
/// </summary>
public sealed class DuplicateGoodsReceiptItemException(DefaultIdType goodsReceiptId, DefaultIdType itemId) : ConflictException($"item {itemId} already exists in goods receipt {goodsReceiptId}");
