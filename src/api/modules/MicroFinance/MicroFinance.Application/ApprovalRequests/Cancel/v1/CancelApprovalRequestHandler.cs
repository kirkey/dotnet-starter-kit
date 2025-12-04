using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.ApprovalRequests.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.ApprovalRequests.Cancel.v1;

/// <summary>
/// Handler to cancel an approval request.
/// </summary>
public sealed class CancelApprovalRequestHandler(
    ILogger<CancelApprovalRequestHandler> logger,
    [FromKeyedServices("microfinance:approvalrequests")] IRepository<ApprovalRequest> repository)
    : IRequestHandler<CancelApprovalRequestCommand, CancelApprovalRequestResponse>
{
    public async Task<CancelApprovalRequestResponse> Handle(CancelApprovalRequestCommand request, CancellationToken cancellationToken)
    {
        var approvalRequest = await repository.FirstOrDefaultAsync(new ApprovalRequestByIdSpec(request.Id), cancellationToken)
            ?? throw new Exception($"Approval request {request.Id} not found");

        approvalRequest.Cancel(request.Reason);
        await repository.UpdateAsync(approvalRequest, cancellationToken);

        logger.LogInformation("Approval request {Id} cancelled. Reason: {Reason}", approvalRequest.Id, request.Reason);

        return new CancelApprovalRequestResponse(approvalRequest.Id, approvalRequest.Status);
    }
}
