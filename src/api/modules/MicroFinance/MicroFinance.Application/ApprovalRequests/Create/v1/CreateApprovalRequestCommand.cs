using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ApprovalRequests.Create.v1;

/// <summary>
/// Command to create a new approval request.
/// </summary>
public sealed record CreateApprovalRequestCommand(
    string RequestNumber,
    Guid WorkflowId,
    string EntityType,
    Guid EntityId,
    int TotalLevels,
    Guid SubmittedById,
    decimal? Amount = null,
    Guid? BranchId = null,
    string? Comments = null,
    int? SlaHours = null) : IRequest<CreateApprovalRequestResponse>;
