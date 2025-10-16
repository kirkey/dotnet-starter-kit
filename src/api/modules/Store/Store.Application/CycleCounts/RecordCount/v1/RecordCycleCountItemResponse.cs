namespace FSH.Starter.WebApi.Store.Application.CycleCounts.RecordCount.v1;

/// <summary>
/// Response after recording a count for a cycle count item.
/// </summary>
public sealed record RecordCycleCountItemResponse(
    /// <summary>
    /// The cycle count item identifier.
    /// </summary>
    DefaultIdType CycleCountItemId,
    
    /// <summary>
    /// The cycle count identifier.
    /// </summary>
    DefaultIdType CycleCountId,
    
    /// <summary>
    /// The system quantity before counting.
    /// </summary>
    int SystemQuantity,
    
    /// <summary>
    /// The physically counted quantity.
    /// </summary>
    int CountedQuantity,
    
    /// <summary>
    /// The variance (Counted - System).
    /// </summary>
    int VarianceQuantity,
    
    /// <summary>
    /// Indicates if the count is accurate (no variance).
    /// </summary>
    bool IsAccurate,
    
    /// <summary>
    /// Indicates if a recount is required.
    /// </summary>
    bool RequiresRecount);

