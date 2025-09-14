using Store.Domain.Exceptions.CycleCount;

namespace FSH.Starter.WebApi.Store.Application.CycleCounts.Complete.v1;

public sealed class CompleteCycleCountHandler(
    ILogger<CompleteCycleCountHandler> logger,
    [FromKeyedServices("store:cycle-counts")] IRepository<CycleCount> repository)
    : IRequestHandler<CompleteCycleCountCommand, CompleteCycleCountResponse>
{
    public async Task<CompleteCycleCountResponse> Handle(CompleteCycleCountCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var cc = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = cc ?? throw new CycleCountNotFoundException(request.Id);
        cc.Complete();
        await repository.UpdateAsync(cc, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("cycle count completed {CycleCountId}", cc.Id);
        return new CompleteCycleCountResponse(cc.Id);
    }
}

