namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Represents a single day entry in a timesheet.
/// One line per day with regular and overtime hours.
/// </summary>
public class TimesheetLine : AuditableEntity, IAggregateRoot
{
    // Domain Constants - Binary Limits (Powers of 2)
    /// <summary>
    /// Maximum length for the project ID field. (50)
    /// </summary>
    public const int ProjectIdMaxLength = 50;

    /// <summary>
    /// Maximum length for the task description field. (2^9 = 512)
    /// </summary>
    public const int TaskDescriptionMaxLength = 512;

    private TimesheetLine() { }

    private TimesheetLine(
        DefaultIdType id,
        DefaultIdType timesheetId,
        DateTime workDate,
        decimal regularHours = 0,
        decimal overtimeHours = 0,
        string? projectId = null,
        string? taskDescription = null)
    {
        Id = id;
        TimesheetId = timesheetId;
        WorkDate = workDate;
        RegularHours = regularHours;
        OvertimeHours = overtimeHours;
        ProjectId = projectId;
        TaskDescription = taskDescription;
        IsBillable = false;
    }

    /// <summary>
    /// The timesheet this line belongs to.
    /// </summary>
    public DefaultIdType TimesheetId { get; private set; }
    public Timesheet Timesheet { get; private set; } = default!;

    /// <summary>
    /// Date of work.
    /// </summary>
    public DateTime WorkDate { get; private set; }

    /// <summary>
    /// Regular hours worked (0-8).
    /// </summary>
    public decimal RegularHours { get; private set; }

    /// <summary>
    /// Overtime hours worked.
    /// </summary>
    public decimal OvertimeHours { get; private set; }

    /// <summary>
    /// Total hours for the day (regular + overtime).
    /// </summary>
    public decimal TotalHours => RegularHours + OvertimeHours;

    /// <summary>
    /// Project ID (for billing/cost allocation).
    /// </summary>
    public string? ProjectId { get; private set; }

    /// <summary>
    /// Task or work description.
    /// </summary>
    public string? TaskDescription { get; private set; }

    /// <summary>
    /// Whether these hours are billable to customer.
    /// </summary>
    public bool IsBillable { get; private set; }

    /// <summary>
    /// Billing rate for the day (if applicable).
    /// </summary>
    public decimal? BillingRate { get; private set; }

    /// <summary>
    /// Creates a new timesheet line.
    /// </summary>
    public static TimesheetLine Create(
        DefaultIdType timesheetId,
        DateTime workDate,
        decimal regularHours = 0,
        decimal overtimeHours = 0,
        string? projectId = null,
        string? taskDescription = null)
    {
        var line = new TimesheetLine(
            DefaultIdType.NewGuid(),
            timesheetId,
            workDate,
            regularHours,
            overtimeHours,
            projectId,
            taskDescription);

        return line;
    }

    /// <summary>
    /// Updates hours for the line.
    /// </summary>
    public TimesheetLine UpdateHours(decimal regularHours, decimal overtimeHours)
    {
        if (regularHours < 0 || overtimeHours < 0)
            throw new ArgumentException("Hours cannot be negative.");

        if (regularHours > 24 || overtimeHours > 24 || (regularHours + overtimeHours) > 24)
            throw new ArgumentException("Total hours cannot exceed 24 hours per day.");

        RegularHours = regularHours;
        OvertimeHours = overtimeHours;

        return this;
    }

    /// <summary>
    /// Sets project and task information.
    /// </summary>
    public TimesheetLine SetProject(string projectId, string? taskDescription = null)
    {
        ProjectId = projectId;
        TaskDescription = taskDescription;
        return this;
    }

    /// <summary>
    /// Marks as billable with rate.
    /// </summary>
    public TimesheetLine MarkAsBillable(decimal billingRate)
    {
        if (billingRate < 0)
            throw new ArgumentException("Billing rate cannot be negative.", nameof(billingRate));

        IsBillable = true;
        BillingRate = billingRate;

        return this;
    }

    /// <summary>
    /// Marks as non-billable.
    /// </summary>
    public TimesheetLine MarkAsNonBillable()
    {
        IsBillable = false;
        BillingRate = null;

        return this;
    }
}

