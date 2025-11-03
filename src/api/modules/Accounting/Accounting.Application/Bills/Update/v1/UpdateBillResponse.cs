namespace Accounting.Application.Bills.Update.v1;

/// <summary>
/// Response after updating a bill.
/// </summary>
/// <param name="BillId">The ID of the updated bill.</param>
public sealed record UpdateBillResponse(DefaultIdType BillId);

