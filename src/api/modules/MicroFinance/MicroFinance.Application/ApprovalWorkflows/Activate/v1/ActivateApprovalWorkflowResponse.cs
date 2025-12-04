namespace FSH.Starter.WebApi.MicroFinance.Application.ApprovalWorkflows.Activate.v1;

/// <summary>
/// Response from activating an approval workflow.
/// </summary>
public sealed record ActivateApprovalWorkflowResponse(Guid Id, bool IsActive);
