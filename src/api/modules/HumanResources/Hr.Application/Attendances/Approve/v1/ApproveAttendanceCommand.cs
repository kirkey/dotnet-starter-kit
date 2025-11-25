namespace FSH.Starter.WebApi.HumanResources.Application.Attendances.Approve.v1;

/// <summary>
/// Command to approve an attendance record by manager.
/// </summary>
public sealed record ApproveAttendanceCommand(
    DefaultIdType Id,
    [property: DefaultValue(null)] string? Comment = null
) : IRequest<ApproveAttendanceResponse>;

/// <summary>
/// Response for approving attendance.
/// </summary>
public sealed record ApproveAttendanceResponse(
    DefaultIdType Id,
    bool IsApproved,
    string? ManagerComment);

