namespace FSH.Starter.WebApi.MicroFinance.Application.ApprovalWorkflows.Deactivate.v1;

/// <summary>
/// Response from deactivating an approval workflow.
/// </summary>
public sealed record DeactivateApprovalWorkflowResponse(Guid Id, bool IsActive);
