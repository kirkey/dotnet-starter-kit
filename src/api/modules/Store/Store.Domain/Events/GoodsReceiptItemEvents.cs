using Store.Domain.Entities;

namespace Store.Domain.Events;

/// <summary>
/// Event raised when a goods receipt item is updated.
/// </summary>
public record GoodsReceiptItemUpdated(GoodsReceiptItem GoodsReceiptItem) : DomainEvent;

/// <summary>
/// Event raised when a goods receipt item is removed.
/// </summary>
public record GoodsReceiptItemRemoved(
    DefaultIdType Id,
    DefaultIdType GoodsReceiptId,
    DefaultIdType ItemId,
    decimal ReceivedQuantity) : DomainEvent;

/// <summary>
/// Event raised when a goods receipt item quantity is adjusted.
/// </summary>
public record GoodsReceiptItemQuantityAdjusted(
    DefaultIdType Id,
    DefaultIdType GoodsReceiptId,
    DefaultIdType ItemId,
    decimal OldReceivedQuantity,
    decimal NewReceivedQuantity,
    decimal NewLineTotal) : DomainEvent;
