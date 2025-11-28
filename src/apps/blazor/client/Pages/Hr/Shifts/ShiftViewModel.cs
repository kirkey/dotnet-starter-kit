namespace FSH.Starter.Blazor.Client.Pages.Hr.Shifts;

/// <summary>
/// View model for Shift CRUD operations.
/// Contains all properties needed for create and update operations.
/// </summary>
public class ShiftViewModel
{
    /// <summary>
    /// Gets or sets the shift ID.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Gets or sets the shift name.
    /// </summary>
    public string? ShiftName { get; set; }

    /// <summary>
    /// Gets or sets the shift start time.
    /// </summary>
    public TimeSpan? StartTime { get; set; } = new TimeSpan(8, 0, 0);

    /// <summary>
    /// Gets or sets the shift end time.
    /// </summary>
    public TimeSpan? EndTime { get; set; } = new TimeSpan(17, 0, 0);

    /// <summary>
    /// Gets or sets whether this is an overnight shift.
    /// </summary>
    public bool IsOvernight { get; set; }

    /// <summary>
    /// Gets or sets the break duration in minutes.
    /// </summary>
    public int BreakDurationMinutes { get; set; } = 30;

    /// <summary>
    /// Gets or sets the shift description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets whether the shift is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets the calculated working hours (excluding breaks).
    /// </summary>
    public decimal WorkingHours
    {
        get
        {
            if (!StartTime.HasValue || !EndTime.HasValue) return 0;
            
            var duration = IsOvernight 
                ? (TimeSpan.FromHours(24) - StartTime.Value + EndTime.Value)
                : (EndTime.Value - StartTime.Value);
            
            var hours = (decimal)duration.TotalMinutes / 60;
            var breakHours = (decimal)BreakDurationMinutes / 60;
            
            return Math.Max(0, hours - breakHours);
        }
    }
}
