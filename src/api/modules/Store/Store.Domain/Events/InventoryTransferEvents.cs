using Store.Domain.Entities;

namespace Store.Domain.Events;

/// <summary>
/// Event raised when a new inventory transfer is created.
/// </summary>
public record InventoryTransferCreated : DomainEvent
{
    public InventoryTransfer InventoryTransfer { get; init; } = null!;
}

/// <summary>
/// Event raised when an item is added to an inventory transfer.
/// </summary>
public record InventoryTransferItemAdded : DomainEvent
{
    public InventoryTransfer InventoryTransfer { get; init; } = null!;
    public DefaultIdType ItemId { get; init; }
}

public record InventoryTransferApproved : DomainEvent
{
    public InventoryTransfer InventoryTransfer { get; init; } = null!;
}

public record InventoryTransferStarted : DomainEvent
{
    public InventoryTransfer InventoryTransfer { get; init; } = null!;
}

public record InventoryTransferCompleted : DomainEvent
{
    public InventoryTransfer InventoryTransfer { get; init; } = null!;
}

public record InventoryTransferCancelled : DomainEvent
{
    public InventoryTransfer InventoryTransfer { get; init; } = null!;
    public string Reason { get; init; } = null!;
}

public record InventoryTransferUpdated : DomainEvent
{
    public InventoryTransfer InventoryTransfer { get; init; } = null!;
}

public record InventoryTransferItemCreated : DomainEvent
{
    public InventoryTransferItem InventoryTransferItem { get; init; } = null!;
}

public record InventoryTransferItemUpdated : DomainEvent
{
    public InventoryTransferItem InventoryTransferItem { get; init; } = null!;
}

public record InventoryTransferInTransit : DomainEvent
{
    public InventoryTransfer InventoryTransfer { get; init; } = null!;
}

public record InventoryTransferItemShipped : DomainEvent
{
    public InventoryTransferItem InventoryTransferItem { get; init; } = null!;
}

public record InventoryTransferItemReceived : DomainEvent
{
    public InventoryTransferItem InventoryTransferItem { get; init; } = null!;
}

public record InventoryTransferShipped : DomainEvent
{
    public InventoryTransfer InventoryTransfer { get; init; } = null!;
}
