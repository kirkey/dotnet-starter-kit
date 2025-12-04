namespace FSH.Starter.WebApi.MicroFinance.Application.ApprovalRequests.Create.v1;

/// <summary>
/// Response from creating an approval request.
/// </summary>
public sealed record CreateApprovalRequestResponse(Guid Id, string RequestNumber, string Status, int CurrentLevel);
