namespace Accounting.Application.TrialBalance.Search.v1;

/// <summary>
/// Response for trial balance search results.
/// </summary>
public sealed record TrialBalanceSearchResponse
{
    public DefaultIdType Id { get; init; }
    public string TrialBalanceNumber { get; init; } = string.Empty;
    public DefaultIdType PeriodId { get; init; }
    public DateTime GeneratedDate { get; init; }
    public DateTime PeriodStartDate { get; init; }
    public DateTime PeriodEndDate { get; init; }
    public decimal TotalDebits { get; init; }
    public decimal TotalCredits { get; init; }
    public bool IsBalanced { get; init; }
    public decimal OutOfBalanceAmount { get; init; }
    public string Status { get; init; } = string.Empty;
    public int AccountCount { get; init; }
    public DateTime? FinalizedDate { get; init; }
    public string? Description { get; init; }
    public DateTime CreatedOn { get; init; }
}

