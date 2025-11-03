namespace Accounting.Application.Bills.Reject.v1;

/// <summary>
/// Response after rejecting a bill.
/// </summary>
/// <param name="BillId">The ID of the rejected bill.</param>
public sealed record RejectBillResponse(DefaultIdType BillId);

