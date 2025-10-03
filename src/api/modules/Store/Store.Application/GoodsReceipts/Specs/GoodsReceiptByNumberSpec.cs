namespace FSH.Starter.WebApi.Store.Application.GoodsReceipts.Specs;

/// <summary>
/// Specification to find a goods receipt by its receipt number.
/// </summary>
public sealed class GoodsReceiptByNumberSpec : Specification<GoodsReceipt>
{
    public GoodsReceiptByNumberSpec(string receiptNumber)
    {
        Query.Where(gr => gr.ReceiptNumber == receiptNumber);
    }
}
