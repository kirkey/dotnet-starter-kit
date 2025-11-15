namespace FSH.Starter.WebApi.HumanResources.Application.ShiftAssignments.Create.v1;

/// <summary>
/// Command to create a shift assignment for an employee.
/// </summary>
public sealed record CreateShiftAssignmentCommand : IRequest<CreateShiftAssignmentResponse>
{
    /// <summary>Employee ID to assign to the shift.</summary>
    public DefaultIdType EmployeeId { get; init; }

    /// <summary>Shift ID to assign to the employee.</summary>
    public DefaultIdType ShiftId { get; init; }

    /// <summary>Start date of the assignment.</summary>
    public DateTime StartDate { get; init; }

    /// <summary>End date of the assignment (null for ongoing).</summary>
    public DateTime? EndDate { get; init; }

    /// <summary>Whether this is a recurring assignment (e.g., every Monday).</summary>
    public bool IsRecurring { get; init; }

    /// <summary>Day of week for recurring assignments (0=Sunday, 6=Saturday).</summary>
    public int? RecurringDayOfWeek { get; init; }

    /// <summary>Additional notes about the assignment.</summary>
    public string? Notes { get; init; }
}

