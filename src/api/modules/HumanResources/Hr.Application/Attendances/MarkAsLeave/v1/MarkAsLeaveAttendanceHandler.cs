namespace FSH.Starter.WebApi.HumanResources.Application.Attendances.MarkAsLeave.v1;

/// <summary>
/// Handler for marking attendance as leave approved.
/// </summary>
public sealed class MarkAsLeaveAttendanceHandler(
    ILogger<MarkAsLeaveAttendanceHandler> logger,
    [FromKeyedServices("hr:attendance")] IRepository<Attendance> repository)
    : IRequestHandler<MarkAsLeaveAttendanceCommand, MarkAsLeaveAttendanceResponse>
{
    public async Task<MarkAsLeaveAttendanceResponse> Handle(
        MarkAsLeaveAttendanceCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var attendance = await repository
            .GetByIdAsync(request.Id, cancellationToken)
            .ConfigureAwait(false);

        if (attendance is null)
            throw new AttendanceNotFoundException(request.Id);

        attendance.MarkAsLeave(request.Reason);

        await repository.UpdateAsync(attendance, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Attendance record {Id} marked as leave. Reason: {Reason}",
            attendance.Id,
            request.Reason ?? "Not specified");

        return new MarkAsLeaveAttendanceResponse(
            attendance.Id,
            attendance.Status,
            attendance.Reason);
    }
}

