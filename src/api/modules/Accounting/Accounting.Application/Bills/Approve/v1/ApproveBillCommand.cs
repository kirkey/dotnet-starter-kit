namespace Accounting.Application.Bills.Approve.v1;

/// <summary>
/// Command to approve a bill for payment processing.
/// The approver is automatically determined from the current user session.
/// </summary>
/// <param name="BillId">The ID of the bill to approve.</param>
public sealed record ApproveBillCommand(
    DefaultIdType BillId
) : IRequest<ApproveBillResponse>;
