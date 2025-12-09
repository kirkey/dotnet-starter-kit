using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.InterestRateChanges.Apply.v1;

/// <summary>
/// Handler for applying an approved interest rate change to the loan.
/// </summary>
public sealed class ApplyInterestRateChangeHandler(
    [FromKeyedServices("microfinance:interestratechanges")] IRepository<InterestRateChange> repository,
    ILogger<ApplyInterestRateChangeHandler> logger)
    : IRequestHandler<ApplyInterestRateChangeCommand, ApplyInterestRateChangeResponse>
{
    public async Task<ApplyInterestRateChangeResponse> Handle(ApplyInterestRateChangeCommand request, CancellationToken cancellationToken)
    {
        var rateChange = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (rateChange is null)
        {
            throw new NotFoundException($"Interest rate change with ID {request.Id} not found.");
        }

        rateChange.Apply();

        await repository.UpdateAsync(rateChange, cancellationToken);

        logger.LogInformation("Interest rate change {InterestRateChangeId} applied to loan {LoanId}. New rate: {NewRate}%",
            request.Id, rateChange.LoanId, rateChange.NewRate);

        return new ApplyInterestRateChangeResponse(
            rateChange.Id,
            rateChange.Status,
            rateChange.LoanId,
            rateChange.NewRate,
            rateChange.AppliedDate!.Value);
    }
}
