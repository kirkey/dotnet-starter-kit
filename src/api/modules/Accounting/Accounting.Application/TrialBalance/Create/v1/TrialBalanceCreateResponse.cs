namespace Accounting.Application.TrialBalance.Create.v1;

/// <summary>
/// Response returned after successfully creating a trial balance.
/// </summary>
public sealed record TrialBalanceCreateResponse
{
    public DefaultIdType Id { get; init; }
    public string TrialBalanceNumber { get; init; } = string.Empty;
    public DateTime GeneratedDate { get; init; }
    public decimal TotalDebits { get; init; }
    public decimal TotalCredits { get; init; }
    public bool IsBalanced { get; init; }
    public string Status { get; init; } = string.Empty;
    public int AccountCount { get; init; }
}

