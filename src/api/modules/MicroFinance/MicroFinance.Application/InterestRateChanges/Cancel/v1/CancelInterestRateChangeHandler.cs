using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.InterestRateChanges.Cancel.v1;

/// <summary>
/// Handler for cancelling an interest rate change.
/// </summary>
public sealed class CancelInterestRateChangeHandler(
    [FromKeyedServices("microfinance:interestratechanges")] IRepository<InterestRateChange> repository,
    ILogger<CancelInterestRateChangeHandler> logger)
    : IRequestHandler<CancelInterestRateChangeCommand, CancelInterestRateChangeResponse>
{
    public async Task<CancelInterestRateChangeResponse> Handle(CancelInterestRateChangeCommand request, CancellationToken cancellationToken)
    {
        var rateChange = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (rateChange is null)
        {
            throw new NotFoundException($"Interest rate change with ID {request.Id} not found.");
        }

        rateChange.Cancel();

        await repository.UpdateAsync(rateChange, cancellationToken);

        logger.LogInformation("Interest rate change {InterestRateChangeId} cancelled", request.Id);

        return new CancelInterestRateChangeResponse(rateChange.Id, rateChange.Status);
    }
}
