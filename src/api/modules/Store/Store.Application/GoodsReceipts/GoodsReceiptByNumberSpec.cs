namespace FSH.Starter.WebApi.Store.Application.GoodsReceipts;

public sealed class GoodsReceiptByNumberSpec : Specification<GoodsReceipt>
{
    public GoodsReceiptByNumberSpec(string receiptNumber)
    {
        Query.Where(x => x.ReceiptNumber == receiptNumber);
    }
}
