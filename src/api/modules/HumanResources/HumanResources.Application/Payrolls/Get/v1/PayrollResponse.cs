namespace FSH.Starter.WebApi.HumanResources.Application.Payrolls.Get.v1;

/// <summary>
/// Response object for Payroll entity details.
/// </summary>
public sealed record PayrollResponse
{
    /// <summary>
    /// Gets the unique identifier of the payroll.
    /// </summary>
    public DefaultIdType Id { get; init; }

    /// <summary>
    /// Gets the start date of the payroll period.
    /// </summary>
    public DateTime StartDate { get; init; }

    /// <summary>
    /// Gets the end date of the payroll period.
    /// </summary>
    public DateTime EndDate { get; init; }

    /// <summary>
    /// Gets the pay frequency (Weekly, BiWeekly, Monthly).
    /// </summary>
    public string PayFrequency { get; init; } = default!;

    /// <summary>
    /// Gets the status (Draft, Processing, Processed, Posted, Paid).
    /// </summary>
    public string Status { get; init; } = default!;

    /// <summary>
    /// Gets the total gross pay for all employees.
    /// </summary>
    public decimal TotalGrossPay { get; init; }

    /// <summary>
    /// Gets the total taxes withheld.
    /// </summary>
    public decimal TotalTaxes { get; init; }

    /// <summary>
    /// Gets the total deductions.
    /// </summary>
    public decimal TotalDeductions { get; init; }

    /// <summary>
    /// Gets the total net pay for all employees.
    /// </summary>
    public decimal TotalNetPay { get; init; }

    /// <summary>
    /// Gets the number of employees in this payroll.
    /// </summary>
    public int EmployeeCount { get; init; }

    /// <summary>
    /// Gets the date payroll was processed.
    /// </summary>
    public DateTime? ProcessedDate { get; init; }

    /// <summary>
    /// Gets the date payroll was posted to GL.
    /// </summary>
    public DateTime? PostedDate { get; init; }

    /// <summary>
    /// Gets the date payment was made.
    /// </summary>
    public DateTime? PaidDate { get; init; }

    /// <summary>
    /// Gets the GL journal entry ID.
    /// </summary>
    public string? JournalEntryId { get; init; }

    /// <summary>
    /// Gets a value indicating whether payroll is locked.
    /// </summary>
    public bool IsLocked { get; init; }

    /// <summary>
    /// Gets notes about the payroll.
    /// </summary>
    public string? Notes { get; init; }
}
