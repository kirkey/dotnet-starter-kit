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

        if (request.ClockInTime.HasValue)
            attendance.ClockIn(request.ClockInTime.Value, request.ClockInLocation);

        if (request.ClockOutTime.HasValue)
        {
            if (!attendance.ClockInTime.HasValue)
                throw new CannotClockOutWithoutClockInException();
            attendance.ClockOut(request.ClockOutTime.Value, request.ClockOutLocation);
        }

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            switch (request.Status)
            {
                case "Late":
                    attendance.MarkAsLate(request.MinutesLate ?? 0, request.Reason);
                    break;
                case "Absent":
                    attendance.MarkAsAbsent(request.Reason);
                    break;
                case "LeaveApproved":
                    attendance.MarkAsLeave(request.Reason);
                    break;
            }
        }

        if (!string.IsNullOrWhiteSpace(request.ManagerComment))
            attendance.Approve(request.ManagerComment);

        await repository.UpdateAsync(attendance, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Attendance {AttendanceId} updated successfully", attendance.Id);

        return new UpdateAttendanceResponse(attendance.Id);
    }
}

