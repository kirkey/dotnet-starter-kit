using Accounting.Application.Meters.Commands;
using Accounting.Domain.Entities;

namespace Accounting.Application.Meters.Handlers;

public class CreateMeterHandler(IRepository<Meter> repository)
    : IRequestHandler<CreateMeterCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreateMeterCommand request, CancellationToken cancellationToken)
    {
        var meter = Meter.Create(
            request.MeterNumber,
            request.MeterType,
            request.Manufacturer,
            request.ModelNumber,
            request.InstallationDate,
            request.Multiplier,
            request.SerialNumber,
            request.Location,
            null, // gps
            request.MemberId,
            request.IsSmartMeter,
            request.CommunicationProtocol,
            request.AccuracyClass,
            request.MeterConfiguration,
            request.Description,
            request.Notes);

        await repository.AddAsync(meter, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        return meter.Id;
    }
}
