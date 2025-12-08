using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ApprovalWorkflows.Get.v1;

/// <summary>
/// Request to get an approval workflow by ID.
/// </summary>
public sealed record GetApprovalWorkflowRequest(DefaultIdType Id) : IRequest<ApprovalWorkflowResponse>;
