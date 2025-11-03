namespace Accounting.Application.Bills.MarkAsPaid.v1;

/// <summary>
/// Response after marking a bill as paid.
/// </summary>
/// <param name="BillId">The ID of the bill marked as paid.</param>
public sealed record MarkBillAsPaidResponse(DefaultIdType BillId);

