using Store.Domain.Exceptions.CycleCount;

namespace FSH.Starter.WebApi.Store.Application.CycleCounts.Get.v1;

/// <summary>
/// Handler for getting a cycle count by ID.
/// Retrieves cycle count details including all items and calculated statistics.
/// </summary>
public sealed class GetCycleCountRequestHandler(
    [FromKeyedServices("store:cycle-counts")] IReadRepository<CycleCount> repository,
    ICacheService cache)
    : IRequestHandler<GetCycleCountRequest, CycleCountResponse>
{
    /// <summary>
    /// Handles the GetCycleCountRequest to retrieve a cycle count by its ID.
    /// </summary>
    /// <param name="request">The request containing the cycle count ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The cycle count response with all details.</returns>
    /// <exception cref="CycleCountNotFoundException">Thrown when the cycle count is not found.</exception>
    public async Task<CycleCountResponse> Handle(GetCycleCountRequest request, CancellationToken cancellationToken)
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

