namespace Accounting.Application.RetainedEarnings.Responses;

/// <summary>
/// Basic retained earnings response for list views.
/// </summary>
public record RetainedEarningsResponse
{
    public DefaultIdType Id { get; init; }
    public int FiscalYear { get; init; }
    public decimal BeginningBalance { get; init; }
    public decimal NetIncome { get; init; }
    public decimal Dividends { get; init; }
    public decimal EndingBalance { get; init; }
    public string Status { get; init; } = string.Empty;
    public bool IsClosed { get; init; }
    public string? Description { get; init; }
}

/// <summary>
/// Detailed retained earnings response for detail views with all properties.
/// </summary>
public record RetainedEarningsDetailsResponse : RetainedEarningsResponse
{
    public decimal CapitalContributions { get; init; }
    public decimal OtherEquityChanges { get; init; }
    public decimal ApproprietedAmount { get; init; }
    public decimal UnappropriatedAmount { get; init; }
    public DateTime FiscalYearStartDate { get; init; }
    public DateTime FiscalYearEndDate { get; init; }
    public DateTime? ClosedDate { get; init; }
    public string? ClosedBy { get; init; }
    public DefaultIdType? RetainedEarningsAccountId { get; init; }
    public int DistributionCount { get; init; }
    public DateTime? LastDistributionDate { get; init; }
    public string? Notes { get; init; }
}
