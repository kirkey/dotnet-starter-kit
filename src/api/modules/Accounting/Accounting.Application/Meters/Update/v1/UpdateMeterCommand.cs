namespace Accounting.Application.Meters.Update.v1;

/// <summary>
/// Command to update a meter.
/// </summary>
public sealed record UpdateMeterCommand(
    DefaultIdType Id,
    string? Location = null,
    string? GpsCoordinates = null,
    DefaultIdType? MemberId = null,
    string? CommunicationProtocol = null,
    string? MeterConfiguration = null,
    string? Description = null,
    string? Notes = null
) : IRequest<DefaultIdType>;

