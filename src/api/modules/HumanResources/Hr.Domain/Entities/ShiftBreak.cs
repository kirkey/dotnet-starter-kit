namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Represents a break period within a shift.
/// Shifts can have multiple breaks (lunch, coffee, etc).
/// </summary>
public class ShiftBreak : AuditableEntity, IAggregateRoot
{
    private ShiftBreak() { }

    private ShiftBreak(
        DefaultIdType id,
        DefaultIdType shiftId,
        TimeSpan startTime,
        TimeSpan endTime,
        string? breakType = null)
    {
        Id = id;
        ShiftId = shiftId;
        StartTime = startTime;
        EndTime = endTime;
        BreakType = breakType ?? "Lunch";
        IsPaid = false;
    }

    /// <summary>
    /// The shift this break belongs to.
    /// </summary>
    public DefaultIdType ShiftId { get; private set; }
    public Shift Shift { get; private set; } = default!;

    /// <summary>
    /// Break start time.
    /// </summary>
    public TimeSpan StartTime { get; private set; }

    /// <summary>
    /// Break end time.
    /// </summary>
    public TimeSpan EndTime { get; private set; }

    /// <summary>
    /// Type of break (Lunch, Coffee, etc).
    /// </summary>
    public string BreakType { get; private set; } = default!;

    /// <summary>
    /// Duration of break in minutes.
    /// </summary>
    public int DurationMinutes => (int)(EndTime - StartTime).TotalMinutes;

    /// <summary>
    /// Whether break is paid.
    /// </summary>
    public bool IsPaid { get; private set; }

    /// <summary>
    /// Creates a new shift break.
    /// </summary>
    public static ShiftBreak Create(
        DefaultIdType shiftId,
        TimeSpan startTime,
        TimeSpan endTime,
        string? breakType = null)
    {
        if (startTime >= endTime)
            throw new ArgumentException("Break end time must be after start time.", nameof(endTime));

        var breakPeriod = new ShiftBreak(
            DefaultIdType.NewGuid(),
            shiftId,
            startTime,
            endTime,
            breakType);

        return breakPeriod;
    }

    /// <summary>
    /// Updates break times.
    /// </summary>
    public ShiftBreak UpdateTimes(TimeSpan startTime, TimeSpan endTime)
    {
        if (startTime >= endTime)
            throw new ArgumentException("Break end time must be after start time.", nameof(endTime));

        StartTime = startTime;
        EndTime = endTime;
        return this;
    }

    /// <summary>
    /// Marks break as paid.
    /// </summary>
    public ShiftBreak MarkAsPaid()
    {
        IsPaid = true;
        return this;
    }

    /// <summary>
    /// Marks break as unpaid.
    /// </summary>
    public ShiftBreak MarkAsUnpaid()
    {
        IsPaid = false;
        return this;
    }
}

/// <summary>
/// Break type constants.
/// </summary>
public static class BreakType
{
    public const string Lunch = "Lunch";
    public const string Coffee = "Coffee";
    public const string Prayer = "Prayer";
    public const string Rest = "Rest";
    public const string Other = "Other";
}

