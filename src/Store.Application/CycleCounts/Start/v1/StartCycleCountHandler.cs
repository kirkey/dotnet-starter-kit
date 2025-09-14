using Store.Domain.Exceptions.CycleCount;

namespace FSH.Starter.WebApi.Store.Application.CycleCounts.Start.v1;

public sealed class StartCycleCountHandler(
    ILogger<StartCycleCountHandler> logger,
    [FromKeyedServices("store:cycle-counts")] IRepository<CycleCount> repository)
    : IRequestHandler<StartCycleCountCommand, StartCycleCountResponse>
{
    public async Task<StartCycleCountResponse> Handle(StartCycleCountCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var cc = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = cc ?? throw new CycleCountNotFoundException(request.Id);
        if (cc.IsOverdue())
        {
            // Optional: log or handle overdue case
        }
        cc.Start();
        await repository.UpdateAsync(cc, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("cycle count started {CycleCountId}", cc.Id);
        return new StartCycleCountResponse(cc.Id);
    }
}

