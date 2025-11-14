namespace FSH.Starter.WebApi.HumanResources.Application.PayComponentRates.Update.v1;

public sealed class UpdatePayComponentRateHandler(
    ILogger<UpdatePayComponentRateHandler> logger,
    [FromKeyedServices("humanresources:paycomponentrates")] IRepository<PayComponentRate> repository)
    : IRequestHandler<UpdatePayComponentRateCommand, UpdatePayComponentRateResponse>
{
    public async Task<UpdatePayComponentRateResponse> Handle(UpdatePayComponentRateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var rate = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = rate ?? throw new PayComponentRateNotFoundException(request.Id);

        // Use the Update method to apply changes
        rate.Update(
            employeeRate: request.EmployeeRate,
            employerRate: request.EmployerRate,
            additionalEmployerRate: request.AdditionalEmployerRate,
            employeeAmount: request.EmployeeAmount,
            employerAmount: request.EmployerAmount,
            taxRate: request.TaxRate,
            baseAmount: request.BaseAmount,
            excessRate: request.ExcessRate,
            description: request.Description);

        // Update effective end date
        if (request.EffectiveEndDate.HasValue)
        {
            rate.SetEffectiveDates(rate.EffectiveStartDate ?? DateTime.UtcNow, request.EffectiveEndDate);
        }


        await repository.UpdateAsync(rate, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Pay component rate with id {RateId} updated.", rate.Id);

        return new UpdatePayComponentRateResponse(rate.Id);
    }
}

