namespace FSH.Starter.WebApi.HumanResources.Application.Attendances.Delete.v1;

/// <summary>
/// Command to delete an attendance record.
/// </summary>
public sealed record DeleteAttendanceCommand(DefaultIdType Id) : IRequest<DeleteAttendanceResponse>;

