namespace Accounting.Application.Bills.Post.v1;

/// <summary>
/// Response after posting a bill.
/// </summary>
/// <param name="BillId">The ID of the posted bill.</param>
public sealed record PostBillResponse(DefaultIdType BillId);

