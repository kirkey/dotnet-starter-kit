using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ApprovalWorkflows.Deactivate.v1;

/// <summary>
/// Command to deactivate an approval workflow.
/// </summary>
public sealed record DeactivateApprovalWorkflowCommand(DefaultIdType Id) : IRequest<DeactivateApprovalWorkflowResponse>;
