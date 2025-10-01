using FSH.Starter.WebApi.Store.Application.CycleCounts.Get.v1;

namespace FSH.Starter.WebApi.Store.Application.CycleCounts.Search.v1;

public sealed class SearchCycleCountsHandler(
    [FromKeyedServices("store:cycle-counts")] IReadRepository<CycleCount> repository)
    : IRequestHandler<SearchCycleCountsCommand, PagedList<CycleCountResponse>>
{
    public async Task<PagedList<CycleCountResponse>> Handle(SearchCycleCountsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchCycleCountsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<CycleCountResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}

