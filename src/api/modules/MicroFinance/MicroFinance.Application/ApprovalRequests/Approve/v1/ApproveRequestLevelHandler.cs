using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.ApprovalRequests.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.ApprovalRequests.Approve.v1;

/// <summary>
/// Handler to approve the current level of an approval request.
/// </summary>
public sealed class ApproveRequestLevelHandler(
    ILogger<ApproveRequestLevelHandler> logger,
    [FromKeyedServices("microfinance:approvalrequests")] IRepository<ApprovalRequest> repository)
    : IRequestHandler<ApproveRequestLevelCommand, ApproveRequestLevelResponse>
{
    public async Task<ApproveRequestLevelResponse> Handle(ApproveRequestLevelCommand request, CancellationToken cancellationToken)
    {
        var approvalRequest = await repository.FirstOrDefaultAsync(new ApprovalRequestByIdSpec(request.Id), cancellationToken)
            ?? throw new Exception($"Approval request {request.Id} not found");

        approvalRequest.ApproveLevel(request.ApproverId, request.Comments);
        await repository.UpdateAsync(approvalRequest, cancellationToken);

        var isFullyApproved = approvalRequest.Status == ApprovalRequest.StatusApproved;
        logger.LogInformation("Approval request {Id} level approved. Status: {Status}, Fully approved: {IsApproved}",
            approvalRequest.Id, approvalRequest.Status, isFullyApproved);

        return new ApproveRequestLevelResponse(
            approvalRequest.Id,
            approvalRequest.Status,
            approvalRequest.CurrentLevel,
            isFullyApproved);
    }
}
