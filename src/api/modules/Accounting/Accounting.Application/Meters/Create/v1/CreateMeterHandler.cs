namespace Accounting.Application.Meters.Create.v1;

/// <summary>
/// Handler for creating a new meter.
/// </summary>
public sealed class CreateMeterHandler(
    [FromKeyedServices("accounting:meters")] IRepository<Meter> repository,
    ILogger<CreateMeterHandler> logger)
    : IRequestHandler<CreateMeterCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreateMeterCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("Creating meter {MeterNumber}", request.MeterNumber);

        // Check for duplicate meter number
        var spec = new Specification<Meter>();
        spec.Query.Where(m => m.MeterNumber == request.MeterNumber);
        var existing = await repository.FirstOrDefaultAsync(spec, cancellationToken);
        
        if (existing != null)
            throw new InvalidOperationException($"Meter number '{request.MeterNumber}' already exists.");

        var meter = Meter.Create(
            request.MeterNumber,
            request.MeterType,
            request.Manufacturer,
            request.ModelNumber,
            request.InstallationDate,
            request.Multiplier,
            request.SerialNumber,
            request.Location,
            request.GpsCoordinates,
            request.MemberId,
            request.IsSmartMeter,
            request.CommunicationProtocol,
            request.AccuracyClass,
            request.MeterConfiguration,
            request.Description,
            request.Notes);

        await repository.AddAsync(meter, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Meter {MeterNumber} created with ID {MeterId}", 
            meter.MeterNumber, meter.Id);
        
        return meter.Id;
    }
}

