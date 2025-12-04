using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.ApprovalRequests.Create.v1;

/// <summary>
/// Handler for creating a new approval request.
/// </summary>
public sealed class CreateApprovalRequestHandler(
    ILogger<CreateApprovalRequestHandler> logger,
    [FromKeyedServices("microfinance:approvalrequests")] IRepository<ApprovalRequest> repository)
    : IRequestHandler<CreateApprovalRequestCommand, CreateApprovalRequestResponse>
{
    public async Task<CreateApprovalRequestResponse> Handle(CreateApprovalRequestCommand request, CancellationToken cancellationToken)
    {
        var approvalRequest = ApprovalRequest.Create(
            request.RequestNumber,
            request.WorkflowId,
            request.EntityType,
            request.EntityId,
            request.TotalLevels,
            request.SubmittedById,
            request.Amount);

        if (request.BranchId.HasValue)
        {
            approvalRequest.FromBranch(request.BranchId.Value);
        }

        if (request.Comments is not null)
        {
            approvalRequest.WithComments(request.Comments);
        }

        if (request.SlaHours.HasValue)
        {
            approvalRequest.WithSla(request.SlaHours.Value);
        }

        await repository.AddAsync(approvalRequest, cancellationToken);
        logger.LogInformation("Approval request {Number} created with ID {Id}", approvalRequest.RequestNumber, approvalRequest.Id);

        return new CreateApprovalRequestResponse(
            approvalRequest.Id,
            approvalRequest.RequestNumber,
            approvalRequest.Status,
            approvalRequest.CurrentLevel);
    }
}
