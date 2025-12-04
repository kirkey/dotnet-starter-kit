using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ApprovalWorkflows.Activate.v1;

/// <summary>
/// Command to activate an approval workflow.
/// </summary>
public sealed record ActivateApprovalWorkflowCommand(Guid Id) : IRequest<ActivateApprovalWorkflowResponse>;
