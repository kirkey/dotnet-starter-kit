namespace FSH.Starter.WebApi.HumanResources.Application.Attendances.Reject.v1;

/// <summary>
/// Command to reject an attendance record by manager.
/// </summary>
public sealed record RejectAttendanceCommand(
    DefaultIdType Id,
    [property: DefaultValue(null)] string? Comment = null
) : IRequest<RejectAttendanceResponse>;

/// <summary>
/// Response for rejecting attendance.
/// </summary>
public sealed record RejectAttendanceResponse(
    DefaultIdType Id,
    bool IsApproved,
    string? ManagerComment);

