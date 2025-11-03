namespace Accounting.Application.Bills.Delete.v1;

/// <summary>
/// Response after deleting a bill.
/// </summary>
/// <param name="BillId">The ID of the deleted bill.</param>
public sealed record DeleteBillResponse(DefaultIdType BillId);

