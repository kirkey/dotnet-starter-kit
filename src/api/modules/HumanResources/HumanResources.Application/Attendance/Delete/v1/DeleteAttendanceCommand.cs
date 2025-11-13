namespace FSH.Starter.WebApi.HumanResources.Application.Attendance.Delete.v1;

public sealed record DeleteAttendanceCommand(DefaultIdType Id) : IRequest<DeleteAttendanceResponse>;

