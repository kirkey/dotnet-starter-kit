namespace FSH.Starter.WebApi.Store.Application.CycleCounts.Cancel.v1;

/// <summary>
/// Command to cancel a cycle count.
/// </summary>
public sealed record CancelCycleCountCommand(
    /// <summary>
    /// The cycle count identifier to cancel.
    /// </summary>
    DefaultIdType Id,
    
    /// <summary>
    /// Reason for cancellation.
    /// </summary>
    string Reason) : IRequest<CancelCycleCountResponse>;
