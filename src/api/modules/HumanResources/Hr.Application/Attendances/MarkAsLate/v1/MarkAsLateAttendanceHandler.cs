namespace FSH.Starter.WebApi.HumanResources.Application.Attendances.MarkAsLate.v1;

/// <summary>
/// Handler for marking attendance as late.
/// Records minutes late and optional reason.
/// </summary>
public sealed class MarkAsLateAttendanceHandler(
    ILogger<MarkAsLateAttendanceHandler> logger,
    [FromKeyedServices("hr:attendance")] IRepository<Attendance> repository)
    : IRequestHandler<MarkAsLateAttendanceCommand, MarkAsLateAttendanceResponse>
{
    public async Task<MarkAsLateAttendanceResponse> Handle(
        MarkAsLateAttendanceCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var attendance = await repository
            .GetByIdAsync(request.Id, cancellationToken)
            .ConfigureAwait(false);

        if (attendance is null)
            throw new AttendanceNotFoundException(request.Id);

        attendance.MarkAsLate(request.MinutesLate, request.Reason);

        await repository.UpdateAsync(attendance, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Attendance record {Id} marked as late. Minutes late: {MinutesLate}, Reason: {Reason}",
            attendance.Id,
            request.MinutesLate,
            request.Reason ?? "Not specified");

        return new MarkAsLateAttendanceResponse(
            attendance.Id,
            attendance.Status,
            attendance.MinutesLate);
    }
}

