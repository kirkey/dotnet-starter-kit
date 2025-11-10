namespace FSH.Starter.WebApi.Store.Application.CycleCounts.Update.v1;

/// <summary>
/// Command to update a cycle count item.
/// Used for updating count details and notes from mobile interface.
/// </summary>
public sealed record UpdateCycleCountItemCommand : IRequest<DefaultIdType>
{
    /// <summary>
    /// The cycle count item identifier.
    /// </summary>
    public DefaultIdType Id { get; init; }

    /// <summary>
    /// The actual counted quantity.
    /// </summary>
    public decimal ActualQuantity { get; init; }

    /// <summary>
    /// Whether the item has been counted.
    /// </summary>
    public bool IsCounted { get; init; }

    /// <summary>
    /// Additional notes about the count.
    /// </summary>
    public string? Notes { get; init; }
}

