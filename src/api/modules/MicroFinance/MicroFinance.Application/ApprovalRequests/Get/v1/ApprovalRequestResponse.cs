namespace FSH.Starter.WebApi.MicroFinance.Application.ApprovalRequests.Get.v1;

/// <summary>
/// Response containing approval request details.
/// </summary>
public sealed record ApprovalRequestResponse(
    DefaultIdType Id,
    string RequestNumber,
    DefaultIdType WorkflowId,
    string EntityType,
    DefaultIdType EntityId,
    decimal? Amount,
    string Status,
    int CurrentLevel,
    int TotalLevels,
    DateTime SubmittedAt,
    DefaultIdType SubmittedById,
    DateTime? CompletedAt,
    DefaultIdType? BranchId,
    DateTime? SlaDueAt,
    string? Comments,
    string? RejectionReason);
