using Accounting.Application.FixedAssets.Dtos;

namespace Accounting.Application.FixedAssets.Search;

public sealed class SearchFixedAssetsHandler(
    [FromKeyedServices("accounting:fixedassets")] IReadRepository<FixedAsset> repository)
    : IRequestHandler<SearchFixedAssetsRequest, PagedList<FixedAssetDto>>
{
    public async Task<PagedList<FixedAssetDto>> Handle(SearchFixedAssetsRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchFixedAssetsSpec(request);
        var list = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<FixedAssetDto>(list, request.PageNumber, request.PageSize, totalCount);
    }
}


