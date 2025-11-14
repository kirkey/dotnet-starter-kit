using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.HumanResources.Domain.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.HumanResources.Application.PayComponentRates.Create.v1;

public sealed class CreatePayComponentRateHandler(
    ILogger<CreatePayComponentRateHandler> logger,
    [FromKeyedServices("humanresources:paycomponentrates")] IRepository<PayComponentRate> repository)
    : IRequestHandler<CreatePayComponentRateCommand, CreatePayComponentRateResponse>
{
    public async Task<CreatePayComponentRateResponse> Handle(CreatePayComponentRateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Determine rate type and create appropriately
        PayComponentRate rate;

        if (request.EmployeeRate.HasValue && request.EmployerRate.HasValue)
        {
            // Contribution rate (SSS, PhilHealth, Pag-IBIG)
            rate = PayComponentRate.CreateContributionRate(
                request.PayComponentId,
                request.MinAmount,
                request.MaxAmount,
                request.EmployeeRate.Value,
                request.EmployerRate.Value,
                request.Year,
                request.AdditionalEmployerRate);
        }
        else if (request.TaxRate.HasValue)
        {
            // Tax bracket
            rate = PayComponentRate.CreateTaxBracket(
                request.PayComponentId,
                request.MinAmount,
                request.MaxAmount,
                request.BaseAmount ?? 0m,
                request.ExcessRate ?? request.TaxRate.Value,
                request.Year);
        }
        else
        {
            // Fixed amount rate
            rate = PayComponentRate.CreateFixedRate(
                request.PayComponentId,
                request.MinAmount,
                request.MaxAmount,
                request.EmployeeAmount,
                request.EmployerAmount,
                request.Year);
        }

        // Set optional properties
        if (request.EffectiveStartDate.HasValue)
        {
            rate.SetEffectiveDates(request.EffectiveStartDate.Value, request.EffectiveEndDate);
        }

        if (!string.IsNullOrWhiteSpace(request.Description))
        {
            rate.Update(description: request.Description);
        }

        await repository.AddAsync(rate, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Pay component rate created {RateId} for component {PayComponentId}, range {MinAmount}-{MaxAmount}, year {Year}",
            rate.Id,
            request.PayComponentId,
            request.MinAmount,
            request.MaxAmount,
            request.Year);

        return new CreatePayComponentRateResponse(rate.Id);
    }
}

