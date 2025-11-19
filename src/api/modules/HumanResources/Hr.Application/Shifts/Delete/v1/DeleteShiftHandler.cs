namespace FSH.Starter.WebApi.HumanResources.Application.Shifts.Delete.v1;

public sealed class DeleteShiftHandler(
    ILogger<DeleteShiftHandler> logger,
    [FromKeyedServices("hr:shifts")] IRepository<Shift> repository)
    : IRequestHandler<DeleteShiftCommand, DeleteShiftResponse>
{
    public async Task<DeleteShiftResponse> Handle(
        DeleteShiftCommand request,
        CancellationToken cancellationToken)
    {
        var shift = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (shift is null)
            throw new ShiftNotFoundException(request.Id);

        await repository.DeleteAsync(shift, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Shift {ShiftId} deleted successfully", shift.Id);

        return new DeleteShiftResponse(shift.Id);
    }
}
