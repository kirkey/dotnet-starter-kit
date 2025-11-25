namespace FSH.Starter.WebApi.HumanResources.Application.Attendances.MarkAsLeave.v1;

/// <summary>
/// Command to mark attendance as leave approved.
/// </summary>
public sealed record MarkAsLeaveAttendanceCommand(
    DefaultIdType Id,
    [property: DefaultValue(null)] string? Reason = null
) : IRequest<MarkAsLeaveAttendanceResponse>;

/// <summary>
/// Response for marking attendance as leave.
/// </summary>
public sealed record MarkAsLeaveAttendanceResponse(
    DefaultIdType Id,
    string Status,
    string? Reason);

