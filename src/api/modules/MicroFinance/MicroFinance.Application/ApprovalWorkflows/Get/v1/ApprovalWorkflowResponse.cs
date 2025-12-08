namespace FSH.Starter.WebApi.MicroFinance.Application.ApprovalWorkflows.Get.v1;

/// <summary>
/// Response containing approval workflow details.
/// </summary>
public sealed record ApprovalWorkflowResponse(
    DefaultIdType Id,
    string Code,
    string Name,
    string? Description,
    string EntityType,
    decimal? MinAmount,
    decimal? MaxAmount,
    DefaultIdType? BranchId,
    int NumberOfLevels,
    bool IsSequential,
    bool IsActive,
    int Priority);
