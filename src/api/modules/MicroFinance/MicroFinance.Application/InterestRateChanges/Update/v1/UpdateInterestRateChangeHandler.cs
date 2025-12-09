using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.InterestRateChanges.Update.v1;

/// <summary>
/// Handler for updating a pending interest rate change.
/// </summary>
public sealed class UpdateInterestRateChangeHandler(
    [FromKeyedServices("microfinance:interestratechanges")] IRepository<InterestRateChange> repository,
    ILogger<UpdateInterestRateChangeHandler> logger)
    : IRequestHandler<UpdateInterestRateChangeCommand, UpdateInterestRateChangeResponse>
{
    public async Task<UpdateInterestRateChangeResponse> Handle(UpdateInterestRateChangeCommand request, CancellationToken cancellationToken)
    {
        var rateChange = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (rateChange is null)
        {
            throw new NotFoundException($"Interest rate change with ID {request.Id} not found.");
        }

        rateChange.Update(
            request.ChangeType,
            request.EffectiveDate,
            request.NewRate,
            request.ChangeReason,
            request.Notes);

        await repository.UpdateAsync(rateChange, cancellationToken);

        logger.LogInformation("Interest rate change {InterestRateChangeId} updated", request.Id);

        return new UpdateInterestRateChangeResponse(rateChange.Id);
    }
}
