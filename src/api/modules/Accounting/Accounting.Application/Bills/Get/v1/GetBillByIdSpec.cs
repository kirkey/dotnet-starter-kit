namespace Accounting.Application.Bills.Get.v1;

/// <summary>
/// Specification to get a bill by ID with related data.
/// </summary>
public sealed class GetBillByIdSpec : Specification<Bill>
{
    public GetBillByIdSpec(DefaultIdType billId)
    {
        Query
            .Where(b => b.Id == billId)
            .Include(b => b.LineItems);
    }
}
