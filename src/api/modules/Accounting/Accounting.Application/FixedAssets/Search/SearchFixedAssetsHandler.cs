using Accounting.Application.FixedAssets.Responses;

namespace Accounting.Application.FixedAssets.Search;

/// <summary>
/// Handler for searching fixed assets with filters and pagination.
/// </summary>
public sealed class SearchFixedAssetsHandler(
    [FromKeyedServices("accounting:fixed-assets")] IReadRepository<FixedAsset> repository)
    : IRequestHandler<SearchFixedAssetsRequest, PagedList<FixedAssetResponse>>
{
    public async Task<PagedList<FixedAssetResponse>> Handle(SearchFixedAssetsRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchFixedAssetsSpec(request);
        var list = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var responses = list.Adapt<List<FixedAssetResponse>>();

        return new PagedList<FixedAssetResponse>(responses, request.PageNumber, request.PageSize, totalCount);
    }
}
