namespace FSH.Starter.WebApi.Store.Application.GoodsReceipts.Delete.v1;

public sealed record DeleteGoodsReceiptCommand : IRequest<DeleteGoodsReceiptResponse>
{
    public DefaultIdType GoodsReceiptId { get; set; }
}
