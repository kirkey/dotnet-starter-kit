namespace Accounting.Application.PrepaidExpenses.Queries;

/// <summary>
/// Prepaid expense data transfer object for list views.
/// </summary>
public record PrepaidExpenseDto
{
    public DefaultIdType Id { get; init; }
    public string PrepaidNumber { get; init; } = string.Empty;
    public decimal TotalAmount { get; init; }
    public decimal AmortizedAmount { get; init; }
    public decimal RemainingAmount { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public string AmortizationSchedule { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public bool IsFullyAmortized { get; init; }
}

/// <summary>
/// Prepaid expense data transfer object for detail view with all properties.
/// </summary>
public record PrepaidExpenseDetailsDto : PrepaidExpenseDto
{
    public decimal MonthlyAmortizationAmount { get; init; }
    public int TotalPeriods { get; init; }
    public int PeriodsAmortized { get; init; }
    public int PeriodsRemaining { get; init; }
    public DateTime? LastAmortizationDate { get; init; }
    public DateTime? NextAmortizationDate { get; init; }
    public DefaultIdType? PrepaidAssetAccountId { get; init; }
    public DefaultIdType? ExpenseAccountId { get; init; }
    public DefaultIdType? VendorId { get; init; }
    public string? VendorName { get; init; }
    public DefaultIdType? PeriodId { get; init; }
    public string? Description { get; init; }
    public string? Notes { get; init; }
}

