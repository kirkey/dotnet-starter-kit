namespace Accounting.Application.Consumptions.Update.v1;

/// <summary>
/// Handler for updating a consumption record.
/// </summary>
public sealed class UpdateConsumptionHandler(
    [FromKeyedServices("accounting:consumptions")] IRepository<Consumption> repository,
    ILogger<UpdateConsumptionHandler> logger)
    : IRequestHandler<UpdateConsumptionCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(UpdateConsumptionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        logger.LogInformation("Updating consumption record {Id}", request.Id);

        var consumption = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (consumption == null)
            throw new NotFoundException($"Consumption record with ID {request.Id} not found");

        consumption.Update(
            null, // currentReading - not updateable
            null, // previousReading - not updateable
            request.ReadingType,
            null, // multiplier - not updateable
            request.ReadingSource,
            request.Description,
            request.Notes);

        await repository.UpdateAsync(consumption, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Consumption record {Id} updated successfully", consumption.Id);
        return consumption.Id;
    }
}

