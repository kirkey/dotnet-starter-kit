using FSH.Starter.WebApi.HumanResources.Domain.Events;

namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Represents a shift template defining work hours and break times.
/// Shifts can be standard (9-5) or special (night shift, rotating, etc).
/// </summary>
/// <remarks>
/// Key Design Points:
/// - Reusable shift templates for scheduling
/// - Multiple break periods per shift
/// - Flexible start/end times
/// - Can be applied to multiple employees
/// 
/// Example:
/// - Morning Shift: 6:00 AM - 2:00 PM (8 hours, 1 x 30min break)
/// - Evening Shift: 2:00 PM - 10:00 PM (8 hours, 1 x 30min break)
/// - Night Shift: 10:00 PM - 6:00 AM (8 hours, 1 x 1hr break)
/// </remarks>
public class Shift : AuditableEntity, IAggregateRoot
{
    private Shift() { }

    private Shift(
        DefaultIdType id,
        string shiftName,
        TimeSpan startTime,
        TimeSpan endTime,
        bool isOvernight = false)
    {
        Id = id;
        ShiftName = shiftName;
        StartTime = startTime;
        EndTime = endTime;
        IsOvernight = isOvernight;
        IsActive = true;
        BreakDurationMinutes = 30;

        QueueDomainEvent(new ShiftCreated { Shift = this });
    }

    /// <summary>
    /// Name of the shift (Morning, Evening, Night, etc).
    /// </summary>
    public string ShiftName { get; private set; } = default!;

    /// <summary>
    /// Shift start time (HH:mm:ss).
    /// </summary>
    public TimeSpan StartTime { get; private set; }

    /// <summary>
    /// Shift end time (HH:mm:ss).
    /// </summary>
    public TimeSpan EndTime { get; private set; }

    /// <summary>
    /// Whether shift spans midnight (overnight shift).
    /// </summary>
    public bool IsOvernight { get; private set; }

    /// <summary>
    /// Total break duration in minutes for the shift.
    /// </summary>
    public int BreakDurationMinutes { get; private set; }

    /// <summary>
    /// Calculated working hours for the shift.
    /// </summary>
    public decimal WorkingHours { get; private set; }

    /// <summary>
    /// Description of the shift.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Whether this shift is active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Break periods within this shift.
    /// </summary>
    public ICollection<ShiftBreak> Breaks { get; private set; } = new List<ShiftBreak>();

    /// <summary>
    /// Shift assignments (employees assigned to this shift).
    /// </summary>
    public ICollection<ShiftAssignment> Assignments { get; private set; } = new List<ShiftAssignment>();

    /// <summary>
    /// Creates a new shift.
    /// </summary>
    public static Shift Create(
        string shiftName,
        TimeSpan startTime,
        TimeSpan endTime,
        bool isOvernight = false)
    {
        if (string.IsNullOrWhiteSpace(shiftName))
            throw new ArgumentException("Shift name is required.", nameof(shiftName));

        if (startTime >= endTime && !isOvernight)
            throw new ArgumentException("End time must be after start time.", nameof(endTime));

        var shift = new Shift(
            DefaultIdType.NewGuid(),
            shiftName,
            startTime,
            endTime,
            isOvernight);

        shift.CalculateWorkingHours();
        return shift;
    }

    /// <summary>
    /// Adds a break period to the shift.
    /// </summary>
    public Shift AddBreak(TimeSpan startTime, TimeSpan endTime)
    {
        var breakPeriod = ShiftBreak.Create(Id, startTime, endTime);
        Breaks.Add(breakPeriod);
        RecalculateBreakDuration();
        CalculateWorkingHours();

        QueueDomainEvent(new ShiftBreakAdded { Shift = this, Break = breakPeriod });
        return this;
    }

    /// <summary>
    /// Removes a break period from the shift.
    /// </summary>
    public Shift RemoveBreak(ShiftBreak breakPeriod)
    {
        Breaks.Remove(breakPeriod);
        RecalculateBreakDuration();
        CalculateWorkingHours();

        QueueDomainEvent(new ShiftBreakRemoved { Shift = this, BreakId = breakPeriod.Id });
        return this;
    }

    /// <summary>
    /// Recalculates total break duration from all breaks.
    /// </summary>
    private void RecalculateBreakDuration()
    {
        BreakDurationMinutes = Breaks.Sum(b => b.DurationMinutes);
    }

    /// <summary>
    /// Calculates working hours based on shift times and breaks.
    /// </summary>
    private void CalculateWorkingHours()
    {
        var totalMinutes = 0;

        if (IsOvernight)
        {
            // Overnight shift: from StartTime to midnight, then midnight to EndTime
            totalMinutes = (int)((TimeSpan.FromHours(24) - StartTime).TotalMinutes + EndTime.TotalMinutes);
        }
        else
        {
            // Regular shift: simple subtraction
            totalMinutes = (int)(EndTime - StartTime).TotalMinutes;
        }

        // Subtract break duration
        totalMinutes -= BreakDurationMinutes;

        WorkingHours = (decimal)totalMinutes / 60;
    }

    /// <summary>
    /// Updates shift basic information.
    /// </summary>
    public Shift Update(
        string? shiftName = null,
        TimeSpan? startTime = null,
        TimeSpan? endTime = null,
        string? description = null)
    {
        if (!string.IsNullOrWhiteSpace(shiftName))
            ShiftName = shiftName;

        if (startTime.HasValue)
            StartTime = startTime.Value;

        if (endTime.HasValue)
            EndTime = endTime.Value;

        if (description != null)
            Description = description;

        CalculateWorkingHours();
        QueueDomainEvent(new ShiftUpdated { Shift = this });
        return this;
    }

    /// <summary>
    /// Sets the break duration for the shift.
    /// </summary>
    public Shift SetBreakDuration(int breakDurationMinutes)
    {
        if (breakDurationMinutes < 0)
            throw new ArgumentException("Break duration cannot be negative.", nameof(breakDurationMinutes));

        BreakDurationMinutes = breakDurationMinutes;
        CalculateWorkingHours();
        return this;
    }

    /// <summary>
    /// Sets the description of the shift.
    /// </summary>
    public Shift SetDescription(string? description)
    {
        Description = description;
        return this;
    }

    /// <summary>
    /// Deactivates the shift.
    /// </summary>
    public Shift Deactivate()
    {
        IsActive = false;
        QueueDomainEvent(new ShiftDeactivated { ShiftId = Id });
        return this;
    }

    /// <summary>
    /// Activates the shift.
    /// </summary>
    public Shift Activate()
    {
        IsActive = true;
        QueueDomainEvent(new ShiftActivated { ShiftId = Id });
        return this;
    }
}

/// <summary>
/// Shift type constants.
/// </summary>
public static class ShiftType
{
    public const string Morning = "Morning";
    public const string Evening = "Evening";
    public const string Night = "Night";
    public const string Rotating = "Rotating";
    public const string Custom = "Custom";
}

