namespace FSH.Starter.WebApi.Store.Application.GoodsReceipts.Search.v1;

public sealed class SearchGoodsReceiptsHandler([FromKeyedServices("store:goodsreceipts")] IReadRepository<GoodsReceipt> repository)
    : IRequestHandler<SearchGoodsReceiptsCommand, PagedList<GoodsReceiptResponse>>
{
    public async Task<PagedList<GoodsReceiptResponse>> Handle(SearchGoodsReceiptsCommand request, CancellationToken cancellationToken)
    {
        var spec = new SearchGoodsReceiptsSpec(request);
        var goodsReceipts = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var goodsReceiptResponses = goodsReceipts.Select(gr => new GoodsReceiptResponse
        {
            Id = gr.Id,
            ReceiptNumber = gr.ReceiptNumber,
            PurchaseOrderId = gr.PurchaseOrderId,
            ReceivedDate = gr.ReceivedDate,
            Status = gr.Status,
            TotalLines = gr.TotalLines,
            ReceivedLines = gr.ReceivedLines,
            Notes = gr.Notes
        }).ToList();

        return new PagedList<GoodsReceiptResponse>(
            goodsReceiptResponses,
            request.PageNumber,
            request.PageSize,
            totalCount);
    }
}
