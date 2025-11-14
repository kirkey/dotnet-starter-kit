namespace FSH.Starter.WebApi.HumanResources.Application.Attendance.Update.v1;

public sealed class UpdateAttendanceHandler(
    ILogger<UpdateAttendanceHandler> logger,
    [FromKeyedServices("hr:attendance")] IRepository<Domain.Entities.Attendance> repository)
    : IRequestHandler<UpdateAttendanceCommand, UpdateAttendanceResponse>
{
    public async Task<UpdateAttendanceResponse> Handle(
        UpdateAttendanceCommand request,
        CancellationToken cancellationToken)
    {
        var attendance = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (attendance is null)
            throw new AttendanceNotFoundException(request.Id);

        if (request.ClockInTime.HasValue || request.ClockOutTime.HasValue)
        {
            var clockIn = request.ClockInTime ?? attendance.ClockInTime;
            var clockOut = request.ClockOutTime ?? attendance.ClockOutTime;

            if (clockOut <= clockIn)
                throw new InvalidClockTimeException(clockIn.Value, clockOut.Value);

            if (request.ClockInTime.HasValue)
                attendance.ClockIn(request.ClockInTime.Value, request.ClockInLocation);

            if (request.ClockOutTime.HasValue)
                attendance.ClockOut(request.ClockOutTime.Value, request.ClockOutLocation);
        }

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            switch (request.Status)
            {
                case "Late":
                    if (request.Reason == null)
                        throw new ArgumentException("Reason is required for late status.");
                    attendance.MarkAsLate(int.Parse(request.Reason));
                    break;

                case "Absent":
                    attendance.MarkAsAbsent(request.Reason);
                    break;

                case "LeaveApproved":
                    attendance.MarkAsLeave(request.Reason);
                    break;

                case "Present":
                    // Just update the status without additional logic
                    break;

                default:
                    throw new InvalidAttendanceStatusException(attendance.Status, request.Status);
            }
        }

        await repository.UpdateAsync(attendance, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Attendance record {AttendanceId} updated successfully", attendance.Id);

        return new UpdateAttendanceResponse(attendance.Id);
    }
}

