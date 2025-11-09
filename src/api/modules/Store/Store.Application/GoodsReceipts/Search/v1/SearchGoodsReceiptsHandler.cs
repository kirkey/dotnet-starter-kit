namespace FSH.Starter.WebApi.Store.Application.GoodsReceipts.Search.v1;

public sealed class SearchGoodsReceiptsHandler([FromKeyedServices("store:goodsreceipts")] IReadRepository<GoodsReceipt> repository)
    : IRequestHandler<SearchGoodsReceiptsCommand, PagedList<GoodsReceiptResponse>>
{
    public async Task<PagedList<GoodsReceiptResponse>> Handle(SearchGoodsReceiptsCommand request, CancellationToken cancellationToken)
    {
        var spec = new SearchGoodsReceiptsSpec(request);
        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<GoodsReceiptResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
