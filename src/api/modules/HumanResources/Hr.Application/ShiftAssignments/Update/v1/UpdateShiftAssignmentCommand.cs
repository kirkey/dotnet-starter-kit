namespace FSH.Starter.WebApi.HumanResources.Application.ShiftAssignments.Update.v1;

/// <summary>
/// Command to update a shift assignment.
/// </summary>
public sealed record UpdateShiftAssignmentCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType Id,
    [property: DefaultValue(null)] DateTime? StartDate = null,
    [property: DefaultValue(null)] DateTime? EndDate = null,
    [property: DefaultValue(null)] bool? IsRecurring = null,
    [property: DefaultValue(null)] int? RecurringDayOfWeek = null,
    [property: DefaultValue(null)] string? Notes = null) : IRequest<UpdateShiftAssignmentResponse>;

