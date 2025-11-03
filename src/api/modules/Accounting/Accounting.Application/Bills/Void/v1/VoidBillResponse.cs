namespace Accounting.Application.Bills.Void.v1;

/// <summary>
/// Response after voiding a bill.
/// </summary>
/// <param name="BillId">The ID of the voided bill.</param>
public sealed record VoidBillResponse(DefaultIdType BillId);

