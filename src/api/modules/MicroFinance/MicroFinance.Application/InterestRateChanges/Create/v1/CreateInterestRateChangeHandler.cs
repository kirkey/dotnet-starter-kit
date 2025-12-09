using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.InterestRateChanges.Create.v1;

/// <summary>
/// Handler for creating a new interest rate change request.
/// </summary>
public sealed class CreateInterestRateChangeHandler(
    [FromKeyedServices("microfinance:interestratechanges")] IRepository<InterestRateChange> repository,
    ILogger<CreateInterestRateChangeHandler> logger)
    : IRequestHandler<CreateInterestRateChangeCommand, CreateInterestRateChangeResponse>
{
    public async Task<CreateInterestRateChangeResponse> Handle(CreateInterestRateChangeCommand request, CancellationToken cancellationToken)
    {
        var rateChange = InterestRateChange.Create(
            request.LoanId,
            request.Reference,
            request.ChangeType,
            request.EffectiveDate,
            request.PreviousRate,
            request.NewRate,
            request.ChangeReason,
            request.RequestDate,
            request.Notes);

        await repository.AddAsync(rateChange, cancellationToken);

        logger.LogInformation("Interest rate change {Reference} created for loan {LoanId}: {PreviousRate}% -> {NewRate}%",
            request.Reference, request.LoanId, request.PreviousRate, request.NewRate);

        return new CreateInterestRateChangeResponse(
            rateChange.Id,
            rateChange.Reference,
            rateChange.ChangeType,
            rateChange.PreviousRate,
            rateChange.NewRate,
            rateChange.Status);
    }
}
