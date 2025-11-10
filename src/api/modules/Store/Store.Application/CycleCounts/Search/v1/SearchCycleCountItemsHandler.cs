namespace FSH.Starter.WebApi.Store.Application.CycleCounts.Search.v1;

/// <summary>
/// Handler for searching cycle count items.
/// Returns detailed item information for mobile counting interface.
/// </summary>
public sealed class SearchCycleCountItemsHandler(
    IReadRepository<CycleCountItem> repository)
    : IRequestHandler<SearchCycleCountItemsRequest, PagedList<CycleCountItemDetailResponse>>
{
    public async Task<PagedList<CycleCountItemDetailResponse>> Handle(
        SearchCycleCountItemsRequest request,
        CancellationToken cancellationToken)
    {
        var spec = new SearchCycleCountItemsSpec(request);

        var items = await repository.ListAsync(spec, cancellationToken);
        var count = await repository.CountAsync(spec, cancellationToken);

        var responses = items.Select(item => new CycleCountItemDetailResponse
        {
            Id = item.Id,
            CycleCountId = item.CycleCountId,
            ItemId = item.ItemId,
            ItemSku = item.Item.Sku,
            ItemBarcode = item.Item.Barcode,
            ItemName = item.Item.Name,
            ItemDescription = item.Item.Description,
            LocationName = item.CycleCount.WarehouseLocation?.Name,
            ExpectedQuantity = item.SystemQuantity,
            ActualQuantity = item.CountedQuantity ?? 0,
            VarianceAmount = item.VarianceQuantity ?? 0,
            VariancePercentage = item.SystemQuantity > 0 
                ? Math.Round((decimal)(item.VarianceQuantity ?? 0) / item.SystemQuantity * 100, 2) 
                : 0,
            IsCounted = item.CountedQuantity.HasValue,
            CountDate = item.CountDate,
            CountedBy = item.CountedBy,
            RequiresRecount = item.RequiresRecount,
            RecountReason = item.RecountReason,
            Notes = item.Notes
        }).ToList();

        return new PagedList<CycleCountItemDetailResponse>(
            responses,
            request.PageNumber,
            request.PageSize,
            count);
    }
}

