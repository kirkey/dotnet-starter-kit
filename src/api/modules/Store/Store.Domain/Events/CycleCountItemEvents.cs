using Store.Domain.Entities;

namespace Store.Domain.Events;

/// <summary>
/// Event raised when a cycle count item is updated.
/// </summary>
public record CycleCountItemUpdated(CycleCountItem CycleCountItem) : DomainEvent;

/// <summary>
/// Event raised when a cycle count item is removed.
/// </summary>
public record CycleCountItemRemoved(
    DefaultIdType Id,
    DefaultIdType CycleCountId,
    DefaultIdType ItemId) : DomainEvent;
