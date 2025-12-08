using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ApprovalRequests.Create.v1;

/// <summary>
/// Command to create a new approval request.
/// </summary>
public sealed record CreateApprovalRequestCommand(
    string RequestNumber,
    DefaultIdType WorkflowId,
    string EntityType,
    DefaultIdType EntityId,
    int TotalLevels,
    DefaultIdType SubmittedById,
    decimal? Amount = null,
    DefaultIdType? BranchId = null,
    string? Comments = null,
    int? SlaHours = null) : IRequest<CreateApprovalRequestResponse>;
