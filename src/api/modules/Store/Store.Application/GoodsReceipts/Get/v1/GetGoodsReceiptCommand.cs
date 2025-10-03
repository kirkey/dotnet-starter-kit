namespace FSH.Starter.WebApi.Store.Application.GoodsReceipts.Get.v1;

public sealed record GetGoodsReceiptCommand : IRequest<GetGoodsReceiptResponse>
{
    public DefaultIdType GoodsReceiptId { get; set; }
}
