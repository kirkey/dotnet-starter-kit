namespace FSH.Starter.WebApi.HumanResources.Application.Attendances.Update.v1;

/// <summary>
/// Handler for updating an attendance record.
/// </summary>
public sealed class UpdateAttendanceHandler(
    ILogger<UpdateAttendanceHandler> logger,
    [FromKeyedServices("hr:attendance")] IRepository<Attendance> repository)
    : IRequestHandler<UpdateAttendanceCommand, UpdateAttendanceResponse>
{
    /// <summary>
    /// Handles the request to update an attendance record.
    /// </summary>
    public async Task<UpdateAttendanceResponse> Handle(
        UpdateAttendanceCommand request,
        CancellationToken cancellationToken)
    {
        var record = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (record is null)
            throw new AttendanceNotFoundException(request.Id);

        if (request.ClockOutTime.HasValue)
            record.ClockOut(request.ClockOutTime.Value, request.ClockOutLocation);

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            switch (request.Status.ToLower())
            {
                case "late":
                    record.MarkAsLate(request.MinutesLate ?? 0, request.Reason);
                    break;
                case "absent":
                    record.MarkAsAbsent(request.Reason);
                    break;
                case "leaveapproved":
                    record.MarkAsLeave(request.Reason);
                    break;
                case "present":
                    // Default status, already set on creation
                    break;
            }
        }

        // Add manager comment if provided
        if (!string.IsNullOrWhiteSpace(request.ManagerComment))
            record.Approve(request.ManagerComment);

        await repository.UpdateAsync(record, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Attendance record {AttendanceId} updated successfully", record.Id);

        return new UpdateAttendanceResponse(record.Id);
    }
}

