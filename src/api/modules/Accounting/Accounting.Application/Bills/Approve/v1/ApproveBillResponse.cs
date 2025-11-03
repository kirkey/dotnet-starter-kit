namespace Accounting.Application.Bills.Approve.v1;

/// <summary>
/// Response after approving a bill.
/// </summary>
/// <param name="BillId">The ID of the approved bill.</param>
public sealed record ApproveBillResponse(DefaultIdType BillId);

