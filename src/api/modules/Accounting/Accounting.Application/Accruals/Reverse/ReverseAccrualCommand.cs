namespace Accounting.Application.Accruals.Reverse;

/// <summary>
/// Command to reverse an accrual by Id on a given reversal date.
/// </summary>
public sealed record ReverseAccrualCommand : IRequest
{
    /// <summary>Accrual identifier.</summary>
    public DefaultIdType Id { get; set; }
    /// <summary>Date the reversal should be posted. Defaults to today (UTC date).</summary>
    public DateTime ReversalDate { get; init; } = DateTime.UtcNow.Date;
}
