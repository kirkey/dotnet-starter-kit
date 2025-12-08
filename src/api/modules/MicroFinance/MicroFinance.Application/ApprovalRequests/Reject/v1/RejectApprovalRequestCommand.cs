using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ApprovalRequests.Reject.v1;

/// <summary>
/// Command to reject an approval request.
/// </summary>
public sealed record RejectApprovalRequestCommand(
    DefaultIdType Id,
    DefaultIdType ApproverId,
    string Reason) : IRequest<RejectApprovalRequestResponse>;
