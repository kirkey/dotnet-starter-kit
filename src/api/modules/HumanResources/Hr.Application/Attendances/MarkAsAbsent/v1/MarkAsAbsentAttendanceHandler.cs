namespace FSH.Starter.WebApi.HumanResources.Application.Attendances.MarkAsAbsent.v1;

/// <summary>
/// Handler for marking attendance as absent.
/// </summary>
public sealed class MarkAsAbsentAttendanceHandler(
    ILogger<MarkAsAbsentAttendanceHandler> logger,
    [FromKeyedServices("hr:attendance")] IRepository<Attendance> repository)
    : IRequestHandler<MarkAsAbsentAttendanceCommand, MarkAsAbsentAttendanceResponse>
{
    public async Task<MarkAsAbsentAttendanceResponse> Handle(
        MarkAsAbsentAttendanceCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var attendance = await repository
            .GetByIdAsync(request.Id, cancellationToken)
            .ConfigureAwait(false);

        if (attendance is null)
            throw new AttendanceNotFoundException(request.Id);

        attendance.MarkAsAbsent(request.Reason);

        await repository.UpdateAsync(attendance, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Attendance record {Id} marked as absent. Reason: {Reason}",
            attendance.Id,
            request.Reason ?? "Not specified");

        return new MarkAsAbsentAttendanceResponse(
            attendance.Id,
            attendance.Status,
            attendance.Reason);
    }
}

