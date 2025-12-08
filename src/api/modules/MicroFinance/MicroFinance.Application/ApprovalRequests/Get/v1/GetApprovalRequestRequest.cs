using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ApprovalRequests.Get.v1;

/// <summary>
/// Request to get an approval request by ID.
/// </summary>
public sealed record GetApprovalRequestRequest(DefaultIdType Id) : IRequest<ApprovalRequestResponse>;
