using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ApprovalWorkflows.Create.v1;

/// <summary>
/// Command to create a new approval workflow.
/// </summary>
public sealed record CreateApprovalWorkflowCommand(
    string Code,
    string Name,
    string EntityType,
    int NumberOfLevels,
    string? Description = null,
    decimal? MinAmount = null,
    decimal? MaxAmount = null,
    DefaultIdType? BranchId = null,
    bool IsSequential = true,
    int Priority = 100) : IRequest<CreateApprovalWorkflowResponse>;
