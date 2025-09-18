using Accounting.Application.Meter.Queries;
using Accounting.Application.Meter.Dtos;

namespace Accounting.Application.Meter.Handlers
{
    public class GetMeterByIdHandler(IReadRepository<Accounting.Domain.Meter> repository)
        : IRequestHandler<GetMeterByIdQuery, MeterDto>
    {
        public async Task<MeterDto> Handle(GetMeterByIdQuery request, CancellationToken cancellationToken)
        {
            var meter = await repository.GetByIdAsync(request.Id, cancellationToken);
            if (meter == null)
                throw new NotFoundException($"Meter with Id {request.Id} not found");

            return new MeterDto
            {
                Id = meter.Id,
                MeterNumber = meter.MeterNumber,
                MeterType = meter.MeterType,
                Manufacturer = meter.Manufacturer,
                ModelNumber = meter.ModelNumber,
                SerialNumber = meter.SerialNumber,
                InstallationDate = meter.InstallationDate,
                LastReading = meter.LastReading,
                LastReadingDate = meter.LastReadingDate,
                Multiplier = meter.Multiplier,
                Status = meter.Status,
                Location = meter.Location,
                GpsCoordinates = meter.GpsCoordinates,
                MemberId = meter.MemberId,
                IsSmartMeter = meter.IsSmartMeter,
                CommunicationProtocol = meter.CommunicationProtocol,
                LastMaintenanceDate = meter.LastMaintenanceDate,
                NextCalibrationDate = meter.NextCalibrationDate,
                AccuracyClass = meter.AccuracyClass,
                MeterConfiguration = meter.MeterConfiguration,
                Description = meter.Description,
                Notes = meter.Notes
            };
        }
    }
}

