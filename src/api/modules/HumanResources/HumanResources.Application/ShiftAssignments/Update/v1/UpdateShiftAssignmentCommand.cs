namespace FSH.Starter.WebApi.HumanResources.Application.ShiftAssignments.Update.v1;

/// <summary>
/// Command to update a shift assignment.
/// </summary>
public sealed record UpdateShiftAssignmentCommand : IRequest<UpdateShiftAssignmentResponse>
{
    /// <summary>Shift assignment ID.</summary>
    public DefaultIdType Id { get; init; }

    /// <summary>Updated start date (optional).</summary>
    public DateTime? StartDate { get; init; }

    /// <summary>Updated end date (optional).</summary>
    public DateTime? EndDate { get; init; }

    /// <summary>Whether to enable recurring (optional).</summary>
    public bool? IsRecurring { get; init; }

    /// <summary>Day of week for recurring (optional).</summary>
    public int? RecurringDayOfWeek { get; init; }

    /// <summary>Updated notes (optional).</summary>
    public string? Notes { get; init; }
}

