namespace FSH.Starter.WebApi.Store.Application.GoodsReceipts.MarkReceived.v1;

public sealed record MarkReceivedCommand : IRequest<MarkReceivedResponse>
{
    public DefaultIdType GoodsReceiptId { get; set; }
}
