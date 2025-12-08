using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ApprovalRequests.Approve.v1;

/// <summary>
/// Command to approve the current level of an approval request.
/// </summary>
public sealed record ApproveRequestLevelCommand(
    DefaultIdType Id,
    DefaultIdType ApproverId,
    string? Comments = null) : IRequest<ApproveRequestLevelResponse>;
