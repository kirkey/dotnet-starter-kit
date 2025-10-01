namespace Store.Domain.Events;

public record GoodsReceiptCreated : DomainEvent { public GoodsReceipt GoodsReceipt { get; init; } = default!; }
public record GoodsReceiptItemAdded : DomainEvent { public GoodsReceipt GoodsReceipt { get; init; } = default!; public GoodsReceiptItem Item { get; init; } = default!; }
public record GoodsReceiptCompleted : DomainEvent { public GoodsReceipt GoodsReceipt { get; init; } = default!; }

