using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.InterestRateChanges.Approve.v1;

/// <summary>
/// Handler for approving an interest rate change.
/// </summary>
public sealed class ApproveInterestRateChangeHandler(
    [FromKeyedServices("microfinance:interestratechanges")] IRepository<InterestRateChange> repository,
    ILogger<ApproveInterestRateChangeHandler> logger)
    : IRequestHandler<ApproveInterestRateChangeCommand, ApproveInterestRateChangeResponse>
{
    public async Task<ApproveInterestRateChangeResponse> Handle(ApproveInterestRateChangeCommand request, CancellationToken cancellationToken)
    {
        var rateChange = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (rateChange is null)
        {
            throw new NotFoundException($"Interest rate change with ID {request.Id} not found.");
        }

        rateChange.Approve(request.ApprovedByUserId, request.ApproverName);

        await repository.UpdateAsync(rateChange, cancellationToken);

        logger.LogInformation("Interest rate change {InterestRateChangeId} approved by {ApproverName}. New rate: {NewRate}%",
            request.Id, request.ApproverName, rateChange.NewRate);

        return new ApproveInterestRateChangeResponse(rateChange.Id, rateChange.Status, rateChange.NewRate);
    }
}
