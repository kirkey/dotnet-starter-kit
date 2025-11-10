namespace Accounting.Application.Meters.Delete.v1;

/// <summary>
/// Handler for deleting a meter.
/// Cannot delete meters with reading history.
/// </summary>
public sealed class DeleteMeterHandler(
    [FromKeyedServices("accounting:meters")] IRepository<Meter> repository,
    ILogger<DeleteMeterHandler> logger)
    : IRequestHandler<DeleteMeterCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(DeleteMeterCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        logger.LogInformation("Deleting meter {Id}", request.Id);

        var meter = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (meter == null)
            throw new NotFoundException($"Meter with ID {request.Id} not found");

        if (meter.Readings.Count > 0)
            throw new InvalidOperationException("Cannot delete meter with reading history.");

        await repository.DeleteAsync(meter, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Meter {MeterNumber} deleted successfully", meter.MeterNumber);
        return request.Id;
    }
}

