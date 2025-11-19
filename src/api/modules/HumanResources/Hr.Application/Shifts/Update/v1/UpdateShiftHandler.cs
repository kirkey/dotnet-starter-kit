namespace FSH.Starter.WebApi.HumanResources.Application.Shifts.Update.v1;

public sealed class UpdateShiftHandler(
    ILogger<UpdateShiftHandler> logger,
    [FromKeyedServices("hr:shifts")] IRepository<Shift> repository)
    : IRequestHandler<UpdateShiftCommand, UpdateShiftResponse>
{
    public async Task<UpdateShiftResponse> Handle(
        UpdateShiftCommand request,
        CancellationToken cancellationToken)
    {
        var shift = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (shift is null)
            throw new ShiftNotFoundException(request.Id);

        if (!string.IsNullOrWhiteSpace(request.ShiftName) || request.StartTime.HasValue || request.EndTime.HasValue)
        {
            shift.Update(
                request.ShiftName,
                request.StartTime,
                request.EndTime);
        }

        if (request.BreakDurationMinutes.HasValue)
            shift.SetBreakDuration(request.BreakDurationMinutes.Value);

        if (!string.IsNullOrWhiteSpace(request.Description))
            shift.SetDescription(request.Description);

        await repository.UpdateAsync(shift, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Shift {ShiftId} updated successfully", shift.Id);

        return new UpdateShiftResponse(shift.Id);
    }
}

