namespace FSH.Starter.WebApi.HumanResources.Application.Attendance.Get.v1;

public sealed record GetAttendanceRequest(DefaultIdType Id) : IRequest<AttendanceResponse>;

