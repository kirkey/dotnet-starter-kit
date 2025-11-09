namespace Accounting.Application.RetainedEarnings.Close.v1;

/// <summary>
/// Command to close a retained earnings fiscal year.
/// </summary>
public sealed record CloseRetainedEarningsCommand : IRequest<DefaultIdType>
{
    /// <summary>
    /// The ID of the retained earnings record to close.
    /// </summary>
    public DefaultIdType Id { get; init; }
    
    /// <summary>
    /// The user who is closing the fiscal year.
    /// </summary>
    public string? ClosedBy { get; init; }
}
