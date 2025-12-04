namespace FSH.Starter.WebApi.MicroFinance.Application.ApprovalWorkflows.Get.v1;

/// <summary>
/// Response containing approval workflow details.
/// </summary>
public sealed record ApprovalWorkflowResponse(
    Guid Id,
    string Code,
    string Name,
    string? Description,
    string EntityType,
    decimal? MinAmount,
    decimal? MaxAmount,
    Guid? BranchId,
    int NumberOfLevels,
    bool IsSequential,
    bool IsActive,
    int Priority);
