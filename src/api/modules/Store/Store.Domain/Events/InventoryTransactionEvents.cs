using Store.Domain.Entities;

namespace Store.Domain.Events;

public record InventoryTransactionCreated : DomainEvent
{
    public InventoryTransaction InventoryTransaction { get; init; } = null!;
}

public record InventoryTransactionApproved : DomainEvent
{
    public InventoryTransaction InventoryTransaction { get; init; } = null!;
}

public record InventoryTransactionRejected : DomainEvent
{
    public InventoryTransaction InventoryTransaction { get; init; } = null!;
    public string RejectedBy { get; init; } = null!;
}

public record InventoryTransactionNotesUpdated : DomainEvent
{
    public InventoryTransaction InventoryTransaction { get; init; } = null!;
}
