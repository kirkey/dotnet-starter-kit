namespace FSH.Starter.WebApi.HumanResources.Application.Attendance.Delete.v1;

public sealed class DeleteAttendanceHandler(
    ILogger<DeleteAttendanceHandler> logger,
    [FromKeyedServices("hr:attendance")] IRepository<Domain.Entities.Attendance> repository)
    : IRequestHandler<DeleteAttendanceCommand, DeleteAttendanceResponse>
{
    public async Task<DeleteAttendanceResponse> Handle(
        DeleteAttendanceCommand request,
        CancellationToken cancellationToken)
    {
        var attendance = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (attendance is null)
            throw new AttendanceNotFoundException(request.Id);

        await repository.DeleteAsync(attendance, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Attendance record {AttendanceId} deleted successfully", attendance.Id);

        return new DeleteAttendanceResponse(attendance.Id);
    }
}

