namespace Accounting.Application.Consumptions.Create.v1;

/// <summary>
/// Handler for creating a new consumption record.
/// </summary>
public sealed class CreateConsumptionHandler(
    [FromKeyedServices("accounting:consumptions")] IRepository<Consumption> repository,
    ILogger<CreateConsumptionHandler> logger)
    : IRequestHandler<CreateConsumptionCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreateConsumptionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("Creating consumption record for meter {MeterId}", request.MeterId);

        var consumption = Consumption.Create(
            request.MeterId,
            request.ReadingDate,
            request.CurrentReading,
            request.PreviousReading,
            request.BillingPeriod,
            request.ReadingType,
            request.Multiplier,
            request.ReadingSource,
            request.Description,
            request.Notes);

        await repository.AddAsync(consumption, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Consumption record created with ID {Id}, KWhUsed: {KWhUsed}", 
            consumption.Id, consumption.KWhUsed);
        
        return consumption.Id;
    }
}

