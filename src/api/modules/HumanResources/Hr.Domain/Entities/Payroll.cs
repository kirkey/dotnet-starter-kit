using FSH.Starter.WebApi.HumanResources.Domain.Events;

namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Represents a payroll period and processing workflow.
/// Aggregates employee pay calculations for a period.
/// </summary>
public class Payroll : AuditableEntity, IAggregateRoot
{
    private Payroll() { }

    private Payroll(
        DefaultIdType id,
        DateTime startDate,
        DateTime endDate,
        string payFrequency = "Monthly")
    {
        Id = id;
        StartDate = startDate;
        EndDate = endDate;
        PayFrequency = payFrequency;
        Status = "Draft";
        IsLocked = false;
        TotalGrossPay = 0;
        TotalTaxes = 0;
        TotalDeductions = 0;
        TotalNetPay = 0;

        QueueDomainEvent(new PayrollCreated { Payroll = this });
    }

    /// <summary>
    /// Payroll period start date.
    /// </summary>
    public DateTime StartDate { get; private set; }

    /// <summary>
    /// Payroll period end date.
    /// </summary>
    public DateTime EndDate { get; private set; }

    /// <summary>
    /// Pay frequency (Weekly, BiWeekly, Monthly).
    /// </summary>
    public string PayFrequency { get; private set; } = default!;

    /// <summary>
    /// Status: Draft, Processing, Processed, Posted, Paid.
    /// </summary>
    public string Status { get; private set; } = default!;

    /// <summary>
    /// Total gross pay for all employees.
    /// </summary>
    public decimal TotalGrossPay { get; private set; }

    /// <summary>
    /// Total taxes withheld.
    /// </summary>
    public decimal TotalTaxes { get; private set; }

    /// <summary>
    /// Total deductions (benefits, garnishments, etc).
    /// </summary>
    public decimal TotalDeductions { get; private set; }

    /// <summary>
    /// Total net pay for all employees.
    /// </summary>
    public decimal TotalNetPay { get; private set; }

    /// <summary>
    /// Number of employees in this payroll.
    /// </summary>
    public int EmployeeCount { get; private set; }

    /// <summary>
    /// Date payroll was processed.
    /// </summary>
    public DateTime? ProcessedDate { get; private set; }

    /// <summary>
    /// Date payroll was posted to GL.
    /// </summary>
    public DateTime? PostedDate { get; private set; }

    /// <summary>
    /// Date payment was made.
    /// </summary>
    public DateTime? PaidDate { get; private set; }

    /// <summary>
    /// GL journal entry ID (if posted).
    /// </summary>
    public string? JournalEntryId { get; private set; }

    /// <summary>
    /// Whether payroll is locked from editing.
    /// </summary>
    public bool IsLocked { get; private set; }

    /// <summary>
    /// Comments or notes about the payroll.
    /// </summary>
    public string? Notes { get; private set; }

    /// <summary>
    /// Payroll lines (one per employee).
    /// </summary>
    public ICollection<PayrollLine> Lines { get; private set; } = new List<PayrollLine>();

    /// <summary>
    /// Creates a new payroll period.
    /// </summary>
    public static Payroll Create(
        DateTime startDate,
        DateTime endDate,
        string payFrequency = "Monthly")
    {
        if (startDate > endDate)
            throw new ArgumentException("Start date must be before end date.", nameof(endDate));

        var payroll = new Payroll(
            DefaultIdType.NewGuid(),
            startDate,
            endDate,
            payFrequency);

        return payroll;
    }

    /// <summary>
    /// Adds a payroll line (employee pay record).
    /// </summary>
    public Payroll AddLine(PayrollLine line)
    {
        if (Status != "Draft")
            throw new InvalidOperationException("Can only add lines to draft payroll.");

        Lines.Add(line);
        return this;
    }

    /// <summary>
    /// Removes a payroll line.
    /// </summary>
    public Payroll RemoveLine(PayrollLine line)
    {
        if (Status != "Draft")
            throw new InvalidOperationException("Can only remove lines from draft payroll.");

        Lines.Remove(line);
        return this;
    }

    /// <summary>
    /// Recalculates payroll totals from lines.
    /// </summary>
    public Payroll RecalculateTotals()
    {
        TotalGrossPay = Lines.Sum(l => l.GrossPay);
        TotalTaxes = Lines.Sum(l => l.TotalTaxes);
        TotalDeductions = Lines.Sum(l => l.TotalDeductions);
        TotalNetPay = Lines.Sum(l => l.NetPay);
        EmployeeCount = Lines.Count;

        return this;
    }

    /// <summary>
    /// Processes the payroll (calculates pay).
    /// </summary>
    public Payroll Process()
    {
        if (Status != "Draft")
            throw new InvalidOperationException("Only draft payroll can be processed.");

        if (Lines.Count == 0)
            throw new InvalidOperationException("Payroll must have at least one line.");

        Status = "Processing";
        ProcessedDate = DateTime.UtcNow;
        RecalculateTotals();

        QueueDomainEvent(new PayrollProcessed { Payroll = this });
        return this;
    }

    /// <summary>
    /// Completes processing and moves to processed state.
    /// </summary>
    public Payroll CompleteProcessing()
    {
        if (Status != "Processing")
            throw new InvalidOperationException("Only processing payroll can be completed.");

        Status = "Processed";
        QueueDomainEvent(new PayrollCompleted { Payroll = this });
        return this;
    }

    /// <summary>
    /// Posts payroll to general ledger.
    /// </summary>
    public Payroll Post(string journalEntryId)
    {
        if (Status != "Processed")
            throw new InvalidOperationException("Only processed payroll can be posted.");

        if (string.IsNullOrWhiteSpace(journalEntryId))
            throw new ArgumentException("Journal entry ID is required.", nameof(journalEntryId));

        Status = "Posted";
        PostedDate = DateTime.UtcNow;
        JournalEntryId = journalEntryId;
        IsLocked = true;

        QueueDomainEvent(new PayrollPosted { Payroll = this });
        return this;
    }

    /// <summary>
    /// Marks payroll as paid.
    /// </summary>
    public Payroll MarkAsPaid()
    {
        if (Status != "Posted")
            throw new InvalidOperationException("Only posted payroll can be marked as paid.");

        Status = "Paid";
        PaidDate = DateTime.UtcNow;

        QueueDomainEvent(new PayrollPaid { Payroll = this });
        return this;
    }
}

/// <summary>
/// Pay frequency constants.
/// </summary>
public static class PayFrequency
{
    public const string Weekly = "Weekly";
    public const string BiWeekly = "BiWeekly";
    public const string SemiMonthly = "SemiMonthly";
    public const string Monthly = "Monthly";
}

/// <summary>
/// Payroll status constants.
/// </summary>
public static class PayrollStatus
{
    public const string Draft = "Draft";
    public const string Processing = "Processing";
    public const string Processed = "Processed";
    public const string Posted = "Posted";
    public const string Paid = "Paid";
}

