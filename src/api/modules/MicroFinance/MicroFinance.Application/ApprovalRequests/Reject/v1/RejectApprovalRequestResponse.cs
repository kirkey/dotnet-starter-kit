namespace FSH.Starter.WebApi.MicroFinance.Application.ApprovalRequests.Reject.v1;

/// <summary>
/// Response from rejecting an approval request.
/// </summary>
public sealed record RejectApprovalRequestResponse(Guid Id, string Status, string RejectionReason);
