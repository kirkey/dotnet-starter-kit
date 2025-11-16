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

        // Apply updates to numeric/percentage fields
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

        // Update effective end date if provided (keep existing start date)
        if (request.EffectiveEndDate.HasValue)
        {
            rate.SetEffectiveDates(rate.EffectiveStartDate, request.EffectiveEndDate);
        }

        await repository.UpdateAsync(rate, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Pay component rate with id {RateId} updated. Effective range: {StartDate} - {EndDate}", rate.Id, rate.EffectiveStartDate, rate.EffectiveEndDate);

        return new UpdatePayComponentRateResponse(rate.Id);
    }
}
