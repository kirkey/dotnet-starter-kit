using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.ApprovalRequests.Get.v1;

/// <summary>
/// Handler to get an approval request by ID.
/// </summary>
public sealed class GetApprovalRequestHandler(
    ILogger<GetApprovalRequestHandler> logger,
    [FromKeyedServices("microfinance:approvalrequests")] IReadRepository<ApprovalRequest> repository)
    : IRequestHandler<GetApprovalRequestRequest, ApprovalRequestResponse>
{
    public async Task<ApprovalRequestResponse> Handle(GetApprovalRequestRequest request, CancellationToken cancellationToken)
    {
        var approvalRequest = await repository.FirstOrDefaultAsync(new ApprovalRequestByIdSpec(request.Id), cancellationToken)
            ?? throw new Exception($"Approval request {request.Id} not found");

        logger.LogInformation("Retrieved approval request {Id}", approvalRequest.Id);

        return new ApprovalRequestResponse(
            approvalRequest.Id,
            approvalRequest.RequestNumber,
            approvalRequest.WorkflowId,
            approvalRequest.EntityType,
            approvalRequest.EntityId,
            approvalRequest.Amount,
            approvalRequest.Status,
            approvalRequest.CurrentLevel,
            approvalRequest.TotalLevels,
            approvalRequest.SubmittedAt,
            approvalRequest.SubmittedById,
            approvalRequest.CompletedAt,
            approvalRequest.BranchId,
            approvalRequest.SlaDueAt,
            approvalRequest.Comments,
            approvalRequest.RejectionReason);
    }
}
