using FSH.Starter.WebApi.HumanResources.Application.Attendance.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.Attendance.Get.v1;

public sealed class GetAttendanceHandler(
    [FromKeyedServices("hr:attendance")] IReadRepository<Domain.Entities.Attendance> repository)
    : IRequestHandler<GetAttendanceRequest, AttendanceResponse>
{
    public async Task<AttendanceResponse> Handle(
        GetAttendanceRequest request,
        CancellationToken cancellationToken)
    {
        var attendance = await repository
            .FirstOrDefaultAsync(new AttendanceByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false);

        if (attendance is null)
            throw new AttendanceNotFoundException(request.Id);

        return new AttendanceResponse
        {
            Id = attendance.Id,
            EmployeeId = attendance.EmployeeId,
            AttendanceDate = attendance.AttendanceDate,
            ClockInTime = attendance.ClockInTime,
            ClockOutTime = attendance.ClockOutTime,
            ClockInLocation = attendance.ClockInLocation,
            ClockOutLocation = attendance.ClockOutLocation,
            HoursWorked = attendance.HoursWorked,
            Status = attendance.Status,
            MinutesLate = attendance.MinutesLate,
            Reason = attendance.Reason,
            IsApproved = attendance.IsApproved,
            ManagerComment = attendance.ManagerComment,
            IsActive = attendance.IsActive
        };
    }
}

