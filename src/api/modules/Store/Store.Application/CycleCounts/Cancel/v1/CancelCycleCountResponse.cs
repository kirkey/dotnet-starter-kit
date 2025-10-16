namespace FSH.Starter.WebApi.Store.Application.CycleCounts.Cancel.v1;

/// <summary>
/// Response after cancelling a cycle count.
/// </summary>
public sealed record CancelCycleCountResponse(
    /// <summary>
    /// The cancelled cycle count identifier.
    /// </summary>
    DefaultIdType Id,
    
    /// <summary>
    /// The cancellation reason.
    /// </summary>
    string Reason);

