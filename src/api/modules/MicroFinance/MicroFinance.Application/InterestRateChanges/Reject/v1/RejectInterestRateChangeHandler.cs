using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.InterestRateChanges.Reject.v1;

/// <summary>
/// Handler for rejecting an interest rate change.
/// </summary>
public sealed class RejectInterestRateChangeHandler(
    [FromKeyedServices("microfinance:interestratechanges")] IRepository<InterestRateChange> repository,
    ILogger<RejectInterestRateChangeHandler> logger)
    : IRequestHandler<RejectInterestRateChangeCommand, RejectInterestRateChangeResponse>
{
    public async Task<RejectInterestRateChangeResponse> Handle(RejectInterestRateChangeCommand request, CancellationToken cancellationToken)
    {
        var rateChange = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (rateChange is null)
        {
            throw new NotFoundException($"Interest rate change with ID {request.Id} not found.");
        }

        rateChange.Reject(request.RejectedByUserId, request.RejectorName, request.Reason);

        await repository.UpdateAsync(rateChange, cancellationToken);

        logger.LogInformation("Interest rate change {InterestRateChangeId} rejected by {RejectorName}. Reason: {Reason}",
            request.Id, request.RejectorName, request.Reason);

        return new RejectInterestRateChangeResponse(rateChange.Id, rateChange.Status);
    }
}
