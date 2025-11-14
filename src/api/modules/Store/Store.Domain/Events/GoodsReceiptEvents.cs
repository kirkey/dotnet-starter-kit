using Store.Domain.Entities;

namespace Store.Domain.Events;

/// <summary>
/// Event raised when a new goods receipt is created.
/// </summary>
public record GoodsReceiptCreated : DomainEvent
{
    public GoodsReceipt GoodsReceipt { get; init; } = null!;
}

/// <summary>
/// Event raised when an item is added to a goods receipt.
/// </summary>
public record GoodsReceiptItemAdded : DomainEvent
{
    public GoodsReceipt GoodsReceipt { get; init; } = null!;
    public GoodsReceiptItem Item { get; init; } = null!;
}

/// <summary>
/// Event raised when a goods receipt is completed/marked as received.
/// </summary>
public record GoodsReceiptCompleted : DomainEvent
{
    public GoodsReceipt GoodsReceipt { get; init; } = null!;
}

/// <summary>
/// Event raised when a goods receipt is cancelled.
/// </summary>
public record GoodsReceiptCancelled : DomainEvent
{
    public GoodsReceipt GoodsReceipt { get; init; } = null!;
}

/// <summary>
/// Event raised when a goods receipt status changes.
/// </summary>
public record GoodsReceiptStatusChanged : DomainEvent
{
    public GoodsReceipt GoodsReceipt { get; init; } = null!;
    public string PreviousStatus { get; init; } = null!;
    public string NewStatus { get; init; } = null!;
}

