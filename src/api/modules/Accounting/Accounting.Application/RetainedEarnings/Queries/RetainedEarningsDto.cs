namespace Accounting.Application.RetainedEarnings.Queries;

/// <summary>
/// Retained earnings data transfer object for list views.
/// </summary>
public record RetainedEarningsDto
{
    public DefaultIdType Id { get; init; }
    public int FiscalYear { get; init; }
    public decimal OpeningBalance { get; init; }
    public decimal NetIncome { get; init; }
    public decimal Distributions { get; init; }
    public decimal ClosingBalance { get; init; }
    public string Status { get; init; } = string.Empty;
    public bool IsClosed { get; init; }
    public DateTime FiscalYearStartDate { get; init; }
    public DateTime FiscalYearEndDate { get; init; }
}

/// <summary>
/// Retained earnings data transfer object for detail view with all properties.
/// </summary>
public record RetainedEarningsDetailsDto : RetainedEarningsDto
{
    public decimal CapitalContributions { get; init; }
    public decimal OtherEquityChanges { get; init; }
    public decimal ApproprietedAmount { get; init; }
    public decimal UnappropriatedAmount { get; init; }
    public DateTime? ClosedDate { get; init; }
    public string? ClosedBy { get; init; }
    public DefaultIdType? RetainedEarningsAccountId { get; init; }
    public int DistributionCount { get; init; }
    public DateTime? LastDistributionDate { get; init; }
    public string? Description { get; init; }
    public string? Notes { get; init; }
}

