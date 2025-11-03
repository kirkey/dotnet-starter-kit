namespace Accounting.Application.Bills.Approve.v1;

/// <summary>
/// Command to approve a bill for payment processing.
/// </summary>
/// <param name="BillId">The ID of the bill to approve.</param>
/// <param name="ApprovedBy">User who approved the bill.</param>
public sealed record ApproveBillCommand(
    DefaultIdType BillId,
    string ApprovedBy
) : IRequest<ApproveBillResponse>;
