namespace Accounting.Application.Meters.Create.v1;

/// <summary>
/// Command to create a new meter.
/// </summary>
public sealed record CreateMeterCommand(
    string MeterNumber,
    string MeterType,
    string Manufacturer,
    string ModelNumber,
    DateTime InstallationDate,
    decimal Multiplier = 1,
    string? SerialNumber = null,
    string? Location = null,
    string? GpsCoordinates = null,
    DefaultIdType? MemberId = null,
    bool IsSmartMeter = false,
    string? CommunicationProtocol = null,
    decimal? AccuracyClass = null,
    string? MeterConfiguration = null,
    string? Description = null,
    string? Notes = null
) : IRequest<DefaultIdType>;

