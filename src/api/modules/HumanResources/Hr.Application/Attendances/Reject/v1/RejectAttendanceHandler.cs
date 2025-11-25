namespace FSH.Starter.WebApi.HumanResources.Application.Attendances.Reject.v1;

/// <summary>
/// Handler for rejecting an attendance record by manager.
/// </summary>
public sealed class RejectAttendanceHandler(
    ILogger<RejectAttendanceHandler> logger,
    [FromKeyedServices("hr:attendance")] IRepository<Attendance> repository)
    : IRequestHandler<RejectAttendanceCommand, RejectAttendanceResponse>
{
    public async Task<RejectAttendanceResponse> Handle(
        RejectAttendanceCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var attendance = await repository
            .GetByIdAsync(request.Id, cancellationToken)
            .ConfigureAwait(false);

        if (attendance is null)
            throw new AttendanceNotFoundException(request.Id);

        attendance.Reject(request.Comment);

        await repository.UpdateAsync(attendance, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Attendance record {Id} rejected by manager. Comment: {Comment}",
            attendance.Id,
            request.Comment ?? "None");

        return new RejectAttendanceResponse(
            attendance.Id,
            attendance.IsApproved,
            attendance.ManagerComment);
    }
}

