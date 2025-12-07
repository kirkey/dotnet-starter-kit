using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.ApprovalRequests.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.ApprovalRequests.Reject.v1;

/// <summary>
/// Handler to reject an approval request.
/// </summary>
public sealed class RejectApprovalRequestHandler(
    ILogger<RejectApprovalRequestHandler> logger,
    [FromKeyedServices("microfinance:approvalrequests")] IRepository<ApprovalRequest> repository)
    : IRequestHandler<RejectApprovalRequestCommand, RejectApprovalRequestResponse>
{
    public async Task<RejectApprovalRequestResponse> Handle(RejectApprovalRequestCommand request, CancellationToken cancellationToken)
    {
        var approvalRequest = await repository.FirstOrDefaultAsync(new ApprovalRequestByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Approval request {request.Id} not found");

        approvalRequest.Reject(request.ApproverId, request.Reason);
        await repository.UpdateAsync(approvalRequest, cancellationToken);

        logger.LogInformation("Approval request {Id} rejected. Reason: {Reason}", approvalRequest.Id, request.Reason);

        return new RejectApprovalRequestResponse(
            approvalRequest.Id,
            approvalRequest.Status,
            approvalRequest.RejectionReason!);
    }
}
