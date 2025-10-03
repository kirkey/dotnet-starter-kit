using Store.Domain.Entities;

namespace Store.Domain.Events;

public record InventoryTransactionCreated : DomainEvent
{
    public InventoryTransaction InventoryTransaction { get; init; } = default!;
}

public record InventoryTransactionApproved : DomainEvent
{
    public InventoryTransaction InventoryTransaction { get; init; } = default!;
}

public record InventoryTransactionRejected : DomainEvent
{
    public InventoryTransaction InventoryTransaction { get; init; } = default!;
    public string RejectedBy { get; init; } = default!;
}

public record InventoryTransactionNotesUpdated : DomainEvent
{
    public InventoryTransaction InventoryTransaction { get; init; } = default!;
}
