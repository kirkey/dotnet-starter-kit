using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ApprovalRequests.Reject.v1;

/// <summary>
/// Command to reject an approval request.
/// </summary>
public sealed record RejectApprovalRequestCommand(
    Guid Id,
    Guid ApproverId,
    string Reason) : IRequest<RejectApprovalRequestResponse>;
