namespace Accounting.Application.Bills.Reject.v1;

/// <summary>
/// Command to reject a bill.
/// </summary>
/// <param name="BillId">The ID of the bill to reject.</param>
/// <param name="RejectedBy">User who rejected the bill.</param>
/// <param name="Reason">Reason for rejection.</param>
public sealed record RejectBillCommand(
    DefaultIdType BillId,
    string RejectedBy,
    string Reason
) : IRequest<RejectBillResponse>;
