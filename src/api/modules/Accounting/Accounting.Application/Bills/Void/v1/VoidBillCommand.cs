namespace Accounting.Application.Bills.Void.v1;

/// <summary>
/// Command to void a bill.
/// </summary>
/// <param name="BillId">The ID of the bill to void.</param>
/// <param name="Reason">Reason for voiding the bill.</param>
public sealed record VoidBillCommand(
    DefaultIdType BillId,
    string Reason
) : IRequest<VoidBillResponse>;

