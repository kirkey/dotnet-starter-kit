namespace FSH.Starter.WebApi.HumanResources.Application.Shifts.Get.v1;

/// <summary>
/// Response object for Shift entity details.
/// </summary>
public sealed record ShiftResponse
{
    /// <summary>
    /// Gets the unique identifier of the shift.
    /// </summary>
    public DefaultIdType Id { get; init; }

    /// <summary>
    /// Gets the name of the shift.
    /// </summary>
    public string ShiftName { get; init; } = default!;

    /// <summary>
    /// Gets the start time of the shift.
    /// </summary>
    public TimeSpan StartTime { get; init; }

    /// <summary>
    /// Gets the end time of the shift.
    /// </summary>
    public TimeSpan EndTime { get; init; }

    /// <summary>
    /// Gets a value indicating whether the shift is overnight.
    /// </summary>
    public bool IsOvernight { get; init; }

    /// <summary>
    /// Gets the break duration in minutes.
    /// </summary>
    public int BreakDurationMinutes { get; init; }

    /// <summary>
    /// Gets the total working hours for the shift.
    /// </summary>
    public decimal WorkingHours { get; init; }

    /// <summary>
    /// Gets the description of the shift.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Gets a value indicating whether the shift is active.
    /// </summary>
    public bool IsActive { get; init; }
}

