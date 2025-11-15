namespace FSH.Starter.WebApi.HumanResources.Application.ShiftAssignments.Get.v1;

/// <summary>
/// Response containing shift assignment details.
/// </summary>
public sealed record ShiftAssignmentResponse
{
    /// <summary>Shift assignment ID.</summary>
    public DefaultIdType Id { get; init; }

    /// <summary>Employee ID.</summary>
    public DefaultIdType EmployeeId { get; init; }

    /// <summary>Employee full name.</summary>
    public string? EmployeeName { get; init; }

    /// <summary>Shift ID.</summary>
    public DefaultIdType ShiftId { get; init; }

    /// <summary>Shift name.</summary>
    public string? ShiftName { get; init; }

    /// <summary>Shift start time.</summary>
    public TimeSpan? ShiftStartTime { get; init; }

    /// <summary>Shift end time.</summary>
    public TimeSpan? ShiftEndTime { get; init; }

    /// <summary>Assignment start date.</summary>
    public DateTime StartDate { get; init; }

    /// <summary>Assignment end date (null for ongoing).</summary>
    public DateTime? EndDate { get; init; }

    /// <summary>Whether this is a recurring assignment.</summary>
    public bool IsRecurring { get; init; }

    /// <summary>Day of week for recurring assignments (0=Sunday, 6=Saturday).</summary>
    public int? RecurringDayOfWeek { get; init; }

    /// <summary>Additional notes.</summary>
    public string? Notes { get; init; }

    /// <summary>Whether the assignment is active.</summary>
    public bool IsActive { get; init; }
}

