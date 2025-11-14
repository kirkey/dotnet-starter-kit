namespace FSH.Starter.WebApi.HumanResources.Application.Attendances.Delete.v1;

/// <summary>
/// Handler for deleting an attendance record.
/// </summary>
public sealed class DeleteAttendanceHandler(
    ILogger<DeleteAttendanceHandler> logger,
    [FromKeyedServices("hr:attendance")] IRepository<Attendance> repository)
    : IRequestHandler<DeleteAttendanceCommand, DeleteAttendanceResponse>
{
    /// <summary>
    /// Handles the request to delete an attendance record.
    /// </summary>
    public async Task<DeleteAttendanceResponse> Handle(
        DeleteAttendanceCommand request,
        CancellationToken cancellationToken)
    {
        var record = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (record is null)
            throw new AttendanceNotFoundException(request.Id);

        await repository.DeleteAsync(record, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Attendance record {AttendanceId} deleted successfully", record.Id);

        return new DeleteAttendanceResponse(record.Id);
    }
}
