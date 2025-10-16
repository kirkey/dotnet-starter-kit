namespace FSH.Starter.WebApi.Store.Application.CycleCounts.RecordCount.v1;

/// <summary>
/// Command to record the counted quantity for a specific cycle count item.
/// This is the primary operation during the counting phase.
/// </summary>
public sealed record RecordCycleCountItemCommand(
    /// <summary>
    /// The cycle count identifier.
    /// </summary>
    DefaultIdType CycleCountId,
    
    /// <summary>
    /// The cycle count item identifier.
    /// </summary>
    DefaultIdType CycleCountItemId,
    
    /// <summary>
    /// The physically counted quantity.
    /// </summary>
    int CountedQuantity,
    
    /// <summary>
    /// The identifier of the person who counted the item.
    /// </summary>
    string? CountedBy = null,
    
    /// <summary>
    /// Additional notes about the count.
    /// </summary>
    string? Notes = null) : IRequest<RecordCycleCountItemResponse>;
