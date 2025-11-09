namespace FSH.Starter.Blazor.Client.Pages.Accounting.PrepaidExpenses;

/// <summary>
/// ViewModel for creating or editing prepaid expenses.
/// </summary>
public sealed class PrepaidExpenseViewModel
{
    public DefaultIdType? Id { get; set; }
    public string? PrepaidNumber { get; set; }
    public string? Description { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime? StartDate { get; set; } = DateTime.Today;
    public DateTime? EndDate { get; set; } = DateTime.Today.AddMonths(12);
    public DateTime? PaymentDate { get; set; } = DateTime.Today;
    public string AmortizationSchedule { get; set; } = "Monthly";
    public DefaultIdType? PrepaidAssetAccountId { get; set; }
    public DefaultIdType? ExpenseAccountId { get; set; }
    public DefaultIdType? VendorId { get; set; }
    public string? VendorName { get; set; }
    public DefaultIdType? PaymentId { get; set; }
    public string? Notes { get; set; }
    public string? Status { get; set; }
    public decimal AmortizedAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    public bool IsFullyAmortized { get; set; }
}

