namespace Accounting.Application.Meters.Update.v1;

/// <summary>
/// Handler for updating a meter.
/// </summary>
public sealed class UpdateMeterHandler(
    [FromKeyedServices("accounting:meters")] IRepository<Meter> repository,
    ILogger<UpdateMeterHandler> logger)
    : IRequestHandler<UpdateMeterCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(UpdateMeterCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        logger.LogInformation("Updating meter {Id}", request.Id);

        var meter = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (meter == null)
            throw new NotFoundException($"Meter with ID {request.Id} not found");

        meter.Update(
            request.Location,
            request.GpsCoordinates,
            request.MemberId,
            request.CommunicationProtocol,
            request.MeterConfiguration,
            request.Description,
            request.Notes);

        await repository.UpdateAsync(meter, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Meter {MeterNumber} updated successfully", meter.MeterNumber);
        return meter.Id;
    }
}

