namespace Accounting.Application.PrepaidExpenses.Responses;

/// <summary>
/// Response containing prepaid expense details.
/// </summary>
public record PrepaidExpenseResponse
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
    public DefaultIdType PrepaidAssetAccountId { get; init; }
    public DefaultIdType ExpenseAccountId { get; init; }
    public DefaultIdType? VendorId { get; init; }
    public string? VendorName { get; init; }
    public bool IsFullyAmortized { get; init; }
    public string? Description { get; init; }
    public string? Notes { get; init; }
}

