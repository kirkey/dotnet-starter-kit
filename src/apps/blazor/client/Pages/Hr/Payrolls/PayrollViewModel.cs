namespace FSH.Starter.Blazor.Client.Pages.Hr.Payrolls;

/// <summary>
/// ViewModel for Payroll CRUD operations.
/// </summary>
public class PayrollViewModel
{
    public DefaultIdType Id { get; set; }
    public DateTime? StartDate { get; set; } = DateTime.Today.AddDays(-(DateTime.Today.Day - 1)); // First of current month
    public DateTime? EndDate { get; set; } = DateTime.Today.AddMonths(1).AddDays(-DateTime.Today.Day); // Last of current month
    public string PayFrequency { get; set; } = "Monthly";
    public string? Notes { get; set; }

    // Read-only fields (set by system during processing)
    public string Status { get; set; } = "Draft";
    public decimal TotalGrossPay { get; set; }
    public decimal TotalTaxes { get; set; }
    public decimal TotalDeductions { get; set; }
    public decimal TotalNetPay { get; set; }
    public int EmployeeCount { get; set; }
    public DateTime? ProcessedDate { get; set; }
    public DateTime? PostedDate { get; set; }
    public DateTime? PaidDate { get; set; }
    public string? JournalEntryId { get; set; }
    public bool IsLocked { get; set; }

    public CreatePayrollCommand ToCreateCommand() =>
        new()
        {
            StartDate = StartDate ?? DateTime.Today,
            EndDate = EndDate ?? DateTime.Today.AddMonths(1),
            PayFrequency = PayFrequency,
            Notes = Notes
        };

    public UpdatePayrollCommand ToUpdateCommand() =>
        new()
        {
            Id = Id,
            Status = Status,
            JournalEntryId = JournalEntryId,
            Notes = Notes
        };
}
