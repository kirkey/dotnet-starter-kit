namespace FSH.Starter.WebApi.Store.Application.GoodsReceipts.AddItem.v1;

public sealed record AddGoodsReceiptItemCommand : IRequest<AddGoodsReceiptItemResponse>
{
    public DefaultIdType GoodsReceiptId { get; set; }
    public DefaultIdType ItemId { get; set; }
    public string Name { get; set; } = default!;
    public int Quantity { get; set; }
}
