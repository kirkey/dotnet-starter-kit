using Store.Domain.Exceptions.CycleCount;

namespace FSH.Starter.WebApi.Store.Application.CycleCounts.Get.v1;

public sealed class GetCycleCountHandler(
    [FromKeyedServices("store:cycle-counts")] IReadRepository<CycleCount> repository,
    ICacheService cache)
    : IRequestHandler<GetCycleCountCommand, CycleCountResponse>
{
    public async Task<CycleCountResponse> Handle(GetCycleCountCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await cache.GetOrSetAsync(
            $"cyclecount:{request.Id}",
            async () =>
            {
                var spec = new GetCycleCountSpecs(request.Id);
                var response = await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false) ??
                               throw new CycleCountNotFoundException(request.Id);
                return response;
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);
        return item!;
    }
}

