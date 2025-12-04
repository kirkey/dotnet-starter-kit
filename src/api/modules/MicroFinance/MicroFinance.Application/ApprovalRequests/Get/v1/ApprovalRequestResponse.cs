namespace FSH.Starter.WebApi.MicroFinance.Application.ApprovalRequests.Get.v1;

/// <summary>
/// Response containing approval request details.
/// </summary>
public sealed record ApprovalRequestResponse(
    Guid Id,
    string RequestNumber,
    Guid WorkflowId,
    string EntityType,
    Guid EntityId,
    decimal? Amount,
    string Status,
    int CurrentLevel,
    int TotalLevels,
    DateTime SubmittedAt,
    Guid SubmittedById,
    DateTime? CompletedAt,
    Guid? BranchId,
    DateTime? SlaDueAt,
    string? Comments,
    string? RejectionReason);
