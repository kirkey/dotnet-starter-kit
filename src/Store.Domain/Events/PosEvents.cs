namespace Store.Domain.Events;

public record PosSaleCreated : DomainEvent { public PosSale PosSale { get; init; } = default!; }
public record PosSaleItemAdded : DomainEvent { public PosSale PosSale { get; init; } = default!; public PosSaleItem Item { get; init; } = default!; }
public record PosPaymentAdded : DomainEvent { public PosSale PosSale { get; init; } = default!; public PosPayment Payment { get; init; } = default!; }
public record PosSaleCompleted : DomainEvent { public PosSale PosSale { get; init; } = default!; }
public record PosSaleVoided : DomainEvent { public PosSale PosSale { get; init; } = default!; public string Reason { get; init; } = default!; }

