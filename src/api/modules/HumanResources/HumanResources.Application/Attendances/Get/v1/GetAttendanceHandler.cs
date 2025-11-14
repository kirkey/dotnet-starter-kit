using FSH.Starter.WebApi.HumanResources.Application.Attendances.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.Attendances.Get.v1;

/// <summary>
/// Handler for retrieving an attendance record by ID.
/// </summary>
public sealed class GetAttendanceHandler(
    [FromKeyedServices("hr:attendance")] IReadRepository<Attendance> repository)
    : IRequestHandler<GetAttendanceRequest, AttendanceResponse>
{
    /// <summary>
    /// Handles the request to get an attendance record.
    /// </summary>
    public async Task<AttendanceResponse> Handle(
        GetAttendanceRequest request,
        CancellationToken cancellationToken)
    {
        var record = await repository
            .FirstOrDefaultAsync(new AttendanceByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false);

        if (record is null)
            throw new AttendanceNotFoundException(request.Id);

        return new AttendanceResponse
        {
            Id = record.Id,
            EmployeeId = record.EmployeeId,
            AttendanceDate = record.AttendanceDate,
            ClockInTime = record.ClockInTime,
            ClockOutTime = record.ClockOutTime,
            ClockInLocation = record.ClockInLocation,
            ClockOutLocation = record.ClockOutLocation,
            HoursWorked = record.HoursWorked,
            Status = record.Status,
            MinutesLate = record.MinutesLate,
            Reason = record.Reason,
            IsApproved = record.IsApproved,
            ManagerComment = record.ManagerComment,
            IsActive = record.IsActive
        };
    }
}

