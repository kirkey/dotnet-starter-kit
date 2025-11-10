namespace Accounting.Application.Meters.Delete.v1;

/// <summary>
/// Command to delete a meter.
/// Cannot delete meters with reading history.
/// </summary>
public sealed record DeleteMeterCommand(DefaultIdType Id) : IRequest<DefaultIdType>;

