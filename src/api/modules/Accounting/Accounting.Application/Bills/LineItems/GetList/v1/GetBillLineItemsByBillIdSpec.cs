namespace Accounting.Application.Bills.LineItems.GetList.v1;

/// <summary>
/// Specification to get all line items for a specific bill.
/// </summary>
public sealed class GetBillLineItemsByBillIdSpec : Specification<BillLineItem>
{
    public GetBillLineItemsByBillIdSpec(DefaultIdType billId)
    {
        Query
            .Where(li => li.BillId == billId)
            .OrderBy(li => li.LineNumber);
    }
}
