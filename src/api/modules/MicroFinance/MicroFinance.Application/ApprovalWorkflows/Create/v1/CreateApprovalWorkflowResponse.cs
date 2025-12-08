namespace FSH.Starter.WebApi.MicroFinance.Application.ApprovalWorkflows.Create.v1;

/// <summary>
/// Response from creating an approval workflow.
/// </summary>
public sealed record CreateApprovalWorkflowResponse(DefaultIdType Id, string Code, string Name, string EntityType);
