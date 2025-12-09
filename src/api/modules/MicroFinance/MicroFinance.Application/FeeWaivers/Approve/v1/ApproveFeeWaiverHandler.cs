using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeWaivers.Approve.v1;

/// <summary>
/// Handler for approving a fee waiver.
/// </summary>
public sealed class ApproveFeeWaiverHandler(
    [FromKeyedServices("microfinance:feewaivers")] IRepository<FeeWaiver> repository,
    ILogger<ApproveFeeWaiverHandler> logger)
    : IRequestHandler<ApproveFeeWaiverCommand, ApproveFeeWaiverResponse>
{
    public async Task<ApproveFeeWaiverResponse> Handle(ApproveFeeWaiverCommand request, CancellationToken cancellationToken)
    {
        var feeWaiver = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (feeWaiver is null)
        {
            throw new NotFoundException($"Fee waiver with ID {request.Id} not found.");
        }

        feeWaiver.Approve(request.ApprovedByUserId, request.ApproverName);

        await repository.UpdateAsync(feeWaiver, cancellationToken);

        logger.LogInformation("Fee waiver {FeeWaiverId} approved by {ApproverName}. Waived amount: {WaivedAmount}",
            request.Id, request.ApproverName, feeWaiver.WaivedAmount);

        return new ApproveFeeWaiverResponse(feeWaiver.Id, feeWaiver.Status, feeWaiver.WaivedAmount);
    }
}
