namespace FSH.Starter.WebApi.Store.Application.GoodsReceipts.Create.v1;

public sealed class CreateGoodsReceiptCommand : IRequest<CreateGoodsReceiptResponse>
{
    public string ReceiptNumber { get; set; } = default!;
    public DefaultIdType? PurchaseOrderId { get; set; }
    public DateTime ReceivedDate { get; set; }
}
