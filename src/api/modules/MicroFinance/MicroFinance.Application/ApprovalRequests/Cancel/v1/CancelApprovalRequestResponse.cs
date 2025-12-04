namespace FSH.Starter.WebApi.MicroFinance.Application.ApprovalRequests.Cancel.v1;

/// <summary>
/// Response from cancelling an approval request.
/// </summary>
public sealed record CancelApprovalRequestResponse(Guid Id, string Status);
