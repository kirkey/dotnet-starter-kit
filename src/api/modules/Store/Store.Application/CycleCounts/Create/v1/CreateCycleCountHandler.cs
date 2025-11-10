namespace FSH.Starter.WebApi.Store.Application.CycleCounts.Create.v1;

public sealed class CreateCycleCountHandler(
    ILogger<CreateCycleCountHandler> logger,
    [FromKeyedServices("store:cycle-counts")] IRepository<CycleCount> repository)
    : IRequestHandler<CreateCycleCountCommand, CreateCycleCountResponse>
{
    public async Task<CreateCycleCountResponse> Handle(CreateCycleCountCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var cycleCount = CycleCount.Create(
            request.CountNumber,
            request.WarehouseId,
            request.WarehouseLocationId,
            request.ScheduledDate,
            request.CountType,
            request.CounterName,
            request.SupervisorName,
            request.Notes);

        await repository.AddAsync(cycleCount, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        
        logger.LogInformation("cycle count created {CycleCountId}", cycleCount.Id);
        return new CreateCycleCountResponse(cycleCount.Id);
    }
}
