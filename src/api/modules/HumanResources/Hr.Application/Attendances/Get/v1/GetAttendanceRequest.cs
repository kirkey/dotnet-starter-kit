namespace FSH.Starter.WebApi.HumanResources.Application.Attendances.Get.v1;

/// <summary>
/// Request to get an attendance record by its identifier.
/// </summary>
public sealed record GetAttendanceRequest(DefaultIdType Id) : IRequest<AttendanceResponse>;
