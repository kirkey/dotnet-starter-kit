namespace Accounting.Application.FixedAssets.Search;

public sealed class SearchFixedAssetsHandler(
    [FromKeyedServices("accounting:fixedassets")] IReadRepository<FixedAsset> repository)
    : IRequestHandler<SearchFixedAssetsRequest, PagedList<FixedAssetResponse>>
{
    public async Task<PagedList<FixedAssetResponse>> Handle(SearchFixedAssetsRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchFixedAssetsSpec(request);
        var list = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var responses = list.Select(entity => new FixedAssetResponse(
                entity.PurchaseDate,
                entity.PurchasePrice,
                entity.ServiceLife,
                entity.DepreciationMethodId,
                entity.SalvageValue,
                entity.CurrentBookValue,
                entity.AccumulatedDepreciationAccountId,
                entity.DepreciationExpenseAccountId,
                entity.SerialNumber,
                entity.Location,
                entity.Department,
                entity.IsDisposed,
                entity.DisposalDate,
                entity.DisposalAmount,
                entity.Description)
            {
                Id = entity.Id
                // Note: FixedAssetResponse sets core financial fields via constructor; additional metadata (AssetName, AssetType, Notes)
                // can be included in the response class if needed — keep mapping minimal and stable.
            }).ToList();

        return new PagedList<FixedAssetResponse>(responses, request.PageNumber, request.PageSize, totalCount);
    }
}
