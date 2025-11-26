using FSH.Starter.WebApi.HumanResources.Domain.Events;

namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Represents an employee timesheet for a pay period.
/// Aggregates daily attendance into weekly/bi-weekly timesheet with approval workflow.
/// </summary>
/// <remarks>
/// Key Design Points:
/// - One timesheet per employee per period
/// - Aggregates multiple attendance records
/// - Tracks regular and overtime hours
/// - Project/task allocation for billing
/// - Manager approval workflow
/// - Locked after approval (immutable)
/// 
/// Example:
/// - Employee John Doe for Week of Nov 10-16, 2025
///   - Monday: 8h regular
///   - Tuesday: 8h regular
///   - Wednesday: 8h regular
///   - Thursday: 8h regular
///   - Friday: 10h (2h overtime)
///   - Total: 40 regular, 2 overtime
///   - Status: Submitted (pending approval)
/// </remarks>
public class Timesheet : AuditableEntity, IAggregateRoot
{
    // Domain Constants - Binary Limits (Powers of 2)
    /// <summary>
    /// Maximum length for the period type field. (50)
    /// </summary>
    public const int PeriodTypeMaxLength = 50;

    /// <summary>
    /// Maximum length for the status field. (50)
    /// </summary>
    public const int StatusMaxLength = 50;

    /// <summary>
    /// Maximum length for the manager comment field. (2^9 = 512)
    /// </summary>
    public const int ManagerCommentMaxLength = 512;

    /// <summary>
    /// Maximum length for the rejection reason field. (2^9 = 512)
    /// </summary>
    public const int RejectionReasonMaxLength = 512;

    private Timesheet() { }

    private Timesheet(
        DefaultIdType id,
        DefaultIdType employeeId,
        DateTime startDate,
        DateTime endDate,
        string periodType = "Weekly")
    {
        Id = id;
        EmployeeId = employeeId;
        StartDate = startDate;
        EndDate = endDate;
        PeriodType = periodType;
        Status = "Draft";
        IsLocked = false;
        IsApproved = false;
        RegularHours = 0;
        OvertimeHours = 0;
        TotalHours = 0;

        QueueDomainEvent(new TimesheetCreated { Timesheet = this });
    }

    /// <summary>
    /// The employee this timesheet is for.
    /// </summary>
    public DefaultIdType EmployeeId { get; private set; }
    public Employee Employee { get; private set; } = default!;

    /// <summary>
    /// Start date of the timesheet period.
    /// </summary>
    public DateTime StartDate { get; private set; }

    /// <summary>
    /// End date of the timesheet period.
    /// </summary>
    public DateTime EndDate { get; private set; }

    /// <summary>
    /// Period type (Weekly, BiWeekly, Monthly).
    /// </summary>
    public string PeriodType { get; private set; } = default!;

    /// <summary>
    /// Total regular hours worked.
    /// </summary>
    public decimal RegularHours { get; private set; }

    /// <summary>
    /// Total overtime hours worked.
    /// </summary>
    public decimal OvertimeHours { get; private set; }

    /// <summary>
    /// Total hours (regular + overtime).
    /// </summary>
    public decimal TotalHours { get; private set; }

    /// <summary>
    /// Status: Draft, Submitted, Approved, Rejected, Locked.
    /// </summary>
    public string Status { get; private set; } = default!;

    /// <summary>
    /// Manager assigned to approve this timesheet.
    /// </summary>
    public DefaultIdType? ApproverManagerId { get; private set; }

    /// <summary>
    /// Date timesheet was submitted.
    /// </summary>
    public DateTime? SubmittedDate { get; private set; }

    /// <summary>
    /// Date timesheet was approved.
    /// </summary>
    public DateTime? ApprovedDate { get; private set; }

    /// <summary>
    /// Manager's comments/notes.
    /// </summary>
    public string? ManagerComment { get; private set; }

    /// <summary>
    /// Reason for rejection (if applicable).
    /// </summary>
    public string? RejectionReason { get; private set; }

    /// <summary>
    /// Whether timesheet is locked from further edits.
    /// </summary>
    public bool IsLocked { get; private set; }

    /// <summary>
    /// Whether timesheet is approved.
    /// </summary>
    public bool IsApproved { get; private set; }

    /// <summary>
    /// Timesheet lines (daily breakdown).
    /// </summary>
    public ICollection<TimesheetLine> Lines { get; private set; } = new List<TimesheetLine>();

    /// <summary>
    /// Creates a new timesheet.
    /// </summary>
    public static Timesheet Create(
        DefaultIdType employeeId,
        DateTime startDate,
        DateTime endDate,
        string periodType = "Weekly")
    {
        var timesheet = new Timesheet(
            DefaultIdType.NewGuid(),
            employeeId,
            startDate,
            endDate,
            periodType);

        return timesheet;
    }

    /// <summary>
    /// Adds a timesheet line.
    /// </summary>
    public Timesheet AddLine(TimesheetLine line)
    {
        if (IsLocked)
            throw new InvalidOperationException("Cannot modify locked timesheet.");

        Lines.Add(line);
        RecalculateTotals();

        QueueDomainEvent(new TimesheetLineAdded { Timesheet = this, Line = line });
        return this;
    }

    /// <summary>
    /// Removes a timesheet line.
    /// </summary>
    public Timesheet RemoveLine(TimesheetLine line)
    {
        if (IsLocked)
            throw new InvalidOperationException("Cannot modify locked timesheet.");

        Lines.Remove(line);
        RecalculateTotals();

        QueueDomainEvent(new TimesheetLineRemoved { Timesheet = this, LineId = line.Id });
        return this;
    }

    /// <summary>
    /// Recalculates total hours from lines.
    /// </summary>
    private void RecalculateTotals()
    {
        RegularHours = Lines.Sum(l => l.RegularHours);
        OvertimeHours = Lines.Sum(l => l.OvertimeHours);
        TotalHours = RegularHours + OvertimeHours;
    }

    /// <summary>
    /// Submits timesheet for approval.
    /// </summary>
    public Timesheet Submit(DefaultIdType? approverId = null)
    {
        if (IsLocked)
            throw new InvalidOperationException("Cannot modify locked timesheet.");

        if (Lines.Count == 0)
            throw new InvalidOperationException("Timesheet must have at least one line.");

        Status = "Submitted";
        SubmittedDate = DateTime.UtcNow;
        ApproverManagerId = approverId;

        QueueDomainEvent(new TimesheetSubmitted { Timesheet = this });
        return this;
    }

    /// <summary>
    /// Approves timesheet.
    /// </summary>
    public Timesheet Approve(string? comment = null)
    {
        if (Status != "Submitted")
            throw new InvalidOperationException("Only submitted timesheets can be approved.");

        Status = "Approved";
        IsApproved = true;
        ApprovedDate = DateTime.UtcNow;
        ManagerComment = comment;

        QueueDomainEvent(new TimesheetApproved { Timesheet = this });
        return this;
    }

    /// <summary>
    /// Rejects timesheet.
    /// </summary>
    public Timesheet Reject(string reason)
    {
        if (Status != "Submitted")
            throw new InvalidOperationException("Only submitted timesheets can be rejected.");

        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Rejection reason is required.", nameof(reason));

        Status = "Rejected";
        RejectionReason = reason;

        QueueDomainEvent(new TimesheetRejected { Timesheet = this });
        return this;
    }

    /// <summary>
    /// Locks timesheet from further editing.
    /// </summary>
    public Timesheet Lock()
    {
        IsLocked = true;
        QueueDomainEvent(new TimesheetLocked { TimesheetId = Id });
        return this;
    }

    /// <summary>
    /// Unlocks timesheet for editing.
    /// </summary>
    public Timesheet Unlock()
    {
        IsLocked = false;
        QueueDomainEvent(new TimesheetUnlocked { TimesheetId = Id });
        return this;
    }

    /// <summary>
    /// Resets to draft status.
    /// </summary>
    public Timesheet ResetToDraft()
    {
        if (IsLocked && IsApproved)
            throw new InvalidOperationException("Cannot reset locked and approved timesheet.");

        Status = "Draft";
        IsApproved = false;
        SubmittedDate = null;
        ApprovedDate = null;

        QueueDomainEvent(new TimesheetResetToDraft { Timesheet = this });
        return this;
    }
}

/// <summary>
/// Period type constants.
/// </summary>
public static class TimesheetPeriodType
{
    public const string Weekly = "Weekly";
    public const string BiWeekly = "BiWeekly";
    public const string Monthly = "Monthly";
}

/// <summary>
/// Timesheet status constants.
/// </summary>
public static class TimesheetStatus
{
    public const string Draft = "Draft";
    public const string Submitted = "Submitted";
    public const string Approved = "Approved";
    public const string Rejected = "Rejected";
    public const string Locked = "Locked";
}

