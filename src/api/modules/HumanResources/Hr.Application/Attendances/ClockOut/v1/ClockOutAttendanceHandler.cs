namespace FSH.Starter.WebApi.HumanResources.Application.Attendances.ClockOut.v1;

/// <summary>
/// Handler for clocking out an employee.
/// Records clock out time and calculates hours worked.
/// </summary>
public sealed class ClockOutAttendanceHandler(
    ILogger<ClockOutAttendanceHandler> logger,
    [FromKeyedServices("hr:attendance")] IRepository<Attendance> repository)
    : IRequestHandler<ClockOutAttendanceCommand, ClockOutAttendanceResponse>
{
    public async Task<ClockOutAttendanceResponse> Handle(
        ClockOutAttendanceCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var attendance = await repository
            .GetByIdAsync(request.Id, cancellationToken)
            .ConfigureAwait(false);

        if (attendance is null)
            throw new AttendanceNotFoundException(request.Id);

        // Validate clock out time is after clock in
        if (attendance.ClockInTime.HasValue && request.ClockOutTime < attendance.ClockInTime.Value)
        {
            throw new InvalidClockTimeException(attendance.ClockInTime.Value, request.ClockOutTime);
        }

        attendance.ClockOut(request.ClockOutTime, request.ClockOutLocation);

        await repository.UpdateAsync(attendance, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Attendance record {Id} clocked out at {ClockOutTime}. Hours worked: {HoursWorked}",
            attendance.Id,
            request.ClockOutTime,
            attendance.HoursWorked);

        return new ClockOutAttendanceResponse(
            attendance.Id,
            attendance.ClockOutTime,
            attendance.HoursWorked);
    }
}

