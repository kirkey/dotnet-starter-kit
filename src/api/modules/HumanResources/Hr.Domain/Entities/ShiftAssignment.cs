namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Represents an assignment of a shift to an employee.
/// Tracks when employees are scheduled for specific shifts.
/// </summary>
public class ShiftAssignment : AuditableEntity, IAggregateRoot
{
    private ShiftAssignment() { }

    private ShiftAssignment(
        DefaultIdType id,
        DefaultIdType employeeId,
        DefaultIdType shiftId,
        DateTime startDate,
        DateTime? endDate = null,
        bool isRecurring = false)
    {
        Id = id;
        EmployeeId = employeeId;
        ShiftId = shiftId;
        StartDate = startDate;
        EndDate = endDate;
        IsRecurring = isRecurring;
        IsActive = true;
    }

    /// <summary>
    /// The employee assigned to the shift.
    /// </summary>
    public DefaultIdType EmployeeId { get; private set; }
    public Employee Employee { get; private set; } = default!;

    /// <summary>
    /// The shift assigned to the employee.
    /// </summary>
    public DefaultIdType ShiftId { get; private set; }
    public Shift Shift { get; private set; } = default!;

    /// <summary>
    /// Date the assignment starts.
    /// </summary>
    public DateTime StartDate { get; private set; }

    /// <summary>
    /// Date the assignment ends (null for ongoing).
    /// </summary>
    public DateTime? EndDate { get; private set; }

    /// <summary>
    /// Whether this is a recurring assignment (e.g., every Monday).
    /// </summary>
    public bool IsRecurring { get; private set; }

    /// <summary>
    /// Day of week if recurring (0=Sunday, 6=Saturday).
    /// </summary>
    public int? RecurringDayOfWeek { get; private set; }

    /// <summary>
    /// Whether this assignment is active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Comments or notes about the assignment.
    /// </summary>
    public string? Notes { get; private set; }

    /// <summary>
    /// Creates a new shift assignment.
    /// </summary>
    public static ShiftAssignment Create(
        DefaultIdType employeeId,
        DefaultIdType shiftId,
        DateTime startDate,
        DateTime? endDate = null,
        bool isRecurring = false)
    {
        if (startDate > DateTime.Today && endDate.HasValue && endDate < startDate)
            throw new ArgumentException("End date must be after start date.", nameof(endDate));

        var assignment = new ShiftAssignment(
            DefaultIdType.NewGuid(),
            employeeId,
            shiftId,
            startDate,
            endDate,
            isRecurring);

        return assignment;
    }

    /// <summary>
    /// Updates the assignment dates.
    /// </summary>
    public ShiftAssignment UpdateDates(DateTime startDate, DateTime? endDate = null)
    {
        if (endDate.HasValue && endDate < startDate)
            throw new ArgumentException("End date must be after start date.", nameof(endDate));

        StartDate = startDate;
        EndDate = endDate;

        return this;
    }

    /// <summary>
    /// Sets recurring schedule for specific day of week.
    /// </summary>
    public ShiftAssignment SetRecurring(int dayOfWeek)
    {
        if (dayOfWeek is < 0 or > 6)
            throw new ArgumentException("Day of week must be 0-6 (Sunday=0, Saturday=6).", nameof(dayOfWeek));

        IsRecurring = true;
        RecurringDayOfWeek = dayOfWeek;

        return this;
    }

    /// <summary>
    /// Adds notes to the assignment.
    /// </summary>
    public ShiftAssignment AddNotes(string notes)
    {
        if (!string.IsNullOrWhiteSpace(notes))
            Notes = notes;

        return this;
    }

    /// <summary>
    /// Checks if assignment is active for a given date.
    /// </summary>
    public bool IsActiveOnDate(DateTime date)
    {
        if (!IsActive)
            return false;

        if (date < StartDate)
            return false;

        if (EndDate.HasValue && date > EndDate)
            return false;

        if (IsRecurring && RecurringDayOfWeek.HasValue)
            return (int)date.DayOfWeek == RecurringDayOfWeek;

        return true;
    }

    /// <summary>
    /// Deactivates the assignment.
    /// </summary>
    public ShiftAssignment Deactivate()
    {
        IsActive = false;
        return this;
    }

    /// <summary>
    /// Activates the assignment.
    /// </summary>
    public ShiftAssignment Activate()
    {
        IsActive = true;
        return this;
    }
}

