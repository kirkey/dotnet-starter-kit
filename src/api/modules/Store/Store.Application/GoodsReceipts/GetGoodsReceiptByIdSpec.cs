namespace FSH.Starter.WebApi.Store.Application.GoodsReceipts;

public sealed class GetGoodsReceiptByIdSpec : Specification<GoodsReceipt>
{
    public GetGoodsReceiptByIdSpec(DefaultIdType goodsReceiptId)
    {
        Query
            .Where(x => x.Id == goodsReceiptId)
            .Include(x => x.Items);
    }
}
