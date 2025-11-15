namespace FSH.Starter.WebApi.HumanResources.Application.ShiftAssignments.Create.v1;

/// <summary>
/// Command to create a shift assignment for an employee.
/// </summary>
public sealed record CreateShiftAssignmentCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType EmployeeId,
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType ShiftId,
    [property: DefaultValue("2025-01-01")] DateTime StartDate,
    [property: DefaultValue(null)] DateTime? EndDate = null,
    [property: DefaultValue(false)] bool IsRecurring = false,
    [property: DefaultValue(null)] int? RecurringDayOfWeek = null,
    [property: DefaultValue(null)] string? Notes = null) : IRequest<CreateShiftAssignmentResponse>;

