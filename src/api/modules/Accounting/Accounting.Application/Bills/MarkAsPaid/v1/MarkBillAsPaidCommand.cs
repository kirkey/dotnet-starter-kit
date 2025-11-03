namespace Accounting.Application.Bills.MarkAsPaid.v1;

/// <summary>
/// Command to mark a bill as paid.
/// </summary>
/// <param name="BillId">The ID of the bill to mark as paid.</param>
/// <param name="PaidDate">The date the bill was paid.</param>
public sealed record MarkBillAsPaidCommand(
    DefaultIdType BillId,
    DateTime PaidDate
) : IRequest<MarkBillAsPaidResponse>;

