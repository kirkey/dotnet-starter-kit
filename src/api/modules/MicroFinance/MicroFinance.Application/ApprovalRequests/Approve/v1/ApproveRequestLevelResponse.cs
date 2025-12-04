namespace FSH.Starter.WebApi.MicroFinance.Application.ApprovalRequests.Approve.v1;

/// <summary>
/// Response from approving a level of an approval request.
/// </summary>
public sealed record ApproveRequestLevelResponse(
    Guid Id,
    string Status,
    int CurrentLevel,
    bool IsFullyApproved);
