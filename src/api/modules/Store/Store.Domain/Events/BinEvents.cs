using Store.Domain.Entities;

namespace Store.Domain.Events;

/// <summary>
/// Event raised when a new bin is created in the system.
/// </summary>
public record BinCreated : DomainEvent
{
    public Bin Bin { get; init; } = default!;
}

/// <summary>
/// Event raised when a bin is updated.
/// </summary>
public record BinUpdated : DomainEvent
{
    public Bin Bin { get; init; } = default!;
}

/// <summary>
/// Event raised when bin utilization is updated.
/// </summary>
public record BinUtilizationUpdated : DomainEvent
{
    public Bin Bin { get; init; } = default!;
    public decimal PreviousUtilization { get; init; }
    public decimal NewUtilization { get; init; }
}

/// <summary>
/// Event raised when a bin is activated.
/// </summary>
public record BinActivated : DomainEvent
{
    public Bin Bin { get; init; } = default!;
}

/// <summary>
/// Event raised when a bin is deactivated.
/// </summary>
public record BinDeactivated : DomainEvent
{
    public Bin Bin { get; init; } = default!;
}

