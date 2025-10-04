using Store.Domain.Entities;

namespace Store.Domain.Events;

/// <summary>
/// Event raised when a new inventory transfer is created.
/// </summary>
public record InventoryTransferCreated : DomainEvent
{
    public InventoryTransfer InventoryTransfer { get; init; } = default!;
}

/// <summary>
/// Event raised when an item is added to an inventory transfer.
/// </summary>
public record InventoryTransferItemAdded : DomainEvent
{
    public InventoryTransfer InventoryTransfer { get; init; } = default!;
    public DefaultIdType ItemId { get; init; }
}

public record InventoryTransferApproved : DomainEvent
{
    public InventoryTransfer InventoryTransfer { get; init; } = default!;
}

public record InventoryTransferStarted : DomainEvent
{
    public InventoryTransfer InventoryTransfer { get; init; } = default!;
}

public record InventoryTransferCompleted : DomainEvent
{
    public InventoryTransfer InventoryTransfer { get; init; } = default!;
}

public record InventoryTransferCancelled : DomainEvent
{
    public InventoryTransfer InventoryTransfer { get; init; } = default!;
    public string Reason { get; init; } = default!;
}

public record InventoryTransferUpdated : DomainEvent
{
    public InventoryTransfer InventoryTransfer { get; init; } = default!;
}

public record InventoryTransferItemCreated : DomainEvent
{
    public InventoryTransferItem InventoryTransferItem { get; init; } = default!;
}

public record InventoryTransferItemUpdated : DomainEvent
{
    public InventoryTransferItem InventoryTransferItem { get; init; } = default!;
}

public record InventoryTransferInTransit : DomainEvent
{
    public InventoryTransfer InventoryTransfer { get; init; } = default!;
}

public record InventoryTransferItemShipped : DomainEvent
{
    public InventoryTransferItem InventoryTransferItem { get; init; } = default!;
}

public record InventoryTransferItemReceived : DomainEvent
{
    public InventoryTransferItem InventoryTransferItem { get; init; } = default!;
}

public record InventoryTransferShipped : DomainEvent
{
    public InventoryTransfer InventoryTransfer { get; init; } = default!;
}
