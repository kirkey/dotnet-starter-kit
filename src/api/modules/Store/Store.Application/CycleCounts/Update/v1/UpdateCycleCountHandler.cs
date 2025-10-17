using Store.Domain.Exceptions.CycleCount;

namespace FSH.Starter.WebApi.Store.Application.CycleCounts.Update.v1;

/// <summary>
/// Handler for updating cycle count information.
/// Only updates cycle counts that are in 'Scheduled' status to prevent modifications to active or completed counts.
/// </summary>
public sealed class UpdateCycleCountHandler(
    ILogger<UpdateCycleCountHandler> logger,
    [FromKeyedServices("store:cycle-counts")] IRepository<CycleCount> repository)
    : IRequestHandler<UpdateCycleCountCommand, UpdateCycleCountResponse>
{
    public async Task<UpdateCycleCountResponse> Handle(UpdateCycleCountCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var cycleCount = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = cycleCount ?? throw new CycleCountNotFoundException(request.Id);

        var updatedCycleCount = cycleCount.Update(
            request.WarehouseId,
            request.WarehouseLocationId,
            request.ScheduledDate,
            request.CountType,
            request.Description,
            request.CounterName,
            request.SupervisorName,
            request.Notes);

        await repository.UpdateAsync(updatedCycleCount, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Cycle count with id: {CycleCountId} updated.", cycleCount.Id);

        return new UpdateCycleCountResponse(cycleCount.Id);
    }
}

