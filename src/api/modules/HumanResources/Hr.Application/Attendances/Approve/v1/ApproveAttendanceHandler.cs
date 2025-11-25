namespace FSH.Starter.WebApi.HumanResources.Application.Attendances.Approve.v1;

/// <summary>
/// Handler for approving an attendance record by manager.
/// </summary>
public sealed class ApproveAttendanceHandler(
    ILogger<ApproveAttendanceHandler> logger,
    [FromKeyedServices("hr:attendance")] IRepository<Attendance> repository)
    : IRequestHandler<ApproveAttendanceCommand, ApproveAttendanceResponse>
{
    public async Task<ApproveAttendanceResponse> Handle(
        ApproveAttendanceCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var attendance = await repository
            .GetByIdAsync(request.Id, cancellationToken)
            .ConfigureAwait(false);

        if (attendance is null)
            throw new AttendanceNotFoundException(request.Id);

        attendance.Approve(request.Comment);

        await repository.UpdateAsync(attendance, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Attendance record {Id} approved by manager. Comment: {Comment}",
            attendance.Id,
            request.Comment ?? "None");

        return new ApproveAttendanceResponse(
            attendance.Id,
            attendance.IsApproved,
            attendance.ManagerComment);
    }
}

