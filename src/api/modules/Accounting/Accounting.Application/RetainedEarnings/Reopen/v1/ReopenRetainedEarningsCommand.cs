namespace Accounting.Application.RetainedEarnings.Reopen.v1;

/// <summary>
/// Command to reopen a closed retained earnings fiscal year.
/// </summary>
public sealed record ReopenRetainedEarningsCommand : IRequest<DefaultIdType>
{
    /// <summary>
    /// The ID of the retained earnings record to reopen.
    /// </summary>
    public DefaultIdType Id { get; init; }
    
    /// <summary>
    /// The reason for reopening the fiscal year.
    /// </summary>
    public string? Reason { get; init; }
}
