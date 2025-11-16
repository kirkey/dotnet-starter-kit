namespace FSH.Starter.WebApi.HumanResources.Application.PayComponentRates.Create.v1;

public sealed class CreatePayComponentRateHandler(
    ILogger<CreatePayComponentRateHandler> logger,
    [FromKeyedServices("humanresources:paycomponentrates")] IRepository<PayComponentRate> repository)
    : IRequestHandler<CreatePayComponentRateCommand, CreatePayComponentRateResponse>
{
    public async Task<CreatePayComponentRateResponse> Handle(CreatePayComponentRateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Derive effective dates from Year if explicit start date not supplied
        var startDate = request.EffectiveStartDate ?? new DateTime(request.Year, 1, 1);
        var endDate = request.EffectiveEndDate ?? new DateTime(request.Year, 12, 31);

        // Determine rate type and create appropriately (temporal pattern)
        PayComponentRate rate;

        if (request is { EmployeeRate: not null, EmployerRate: not null })
        {
            // Contribution rate (SSS, PhilHealth, Pag-IBIG)
            rate = PayComponentRate.CreateContributionRate(
                request.PayComponentId,
                request.MinAmount,
                request.MaxAmount,
                request.EmployeeRate.Value,
                request.EmployerRate.Value,
                startDate,
                endDate,
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
                startDate,
                endDate);
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
                startDate,
                endDate);
        }

        // Override effective dates if both explicitly provided and differ
        if (request.EffectiveStartDate.HasValue || request.EffectiveEndDate.HasValue)
        {
            rate.SetEffectiveDates(startDate, request.EffectiveEndDate ?? endDate);
        }

        if (!string.IsNullOrWhiteSpace(request.Description))
        {
            rate.Update(description: request.Description);
        }

        await repository.AddAsync(rate, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Pay component rate created {RateId} for component {PayComponentId}, range {MinAmount}-{MaxAmount}, effective {StartDate} to {EndDate}",
            rate.Id,
            request.PayComponentId,
            request.MinAmount,
            request.MaxAmount,
            rate.EffectiveStartDate,
            rate.EffectiveEndDate);

        return new CreatePayComponentRateResponse(rate.Id);
    }
}
