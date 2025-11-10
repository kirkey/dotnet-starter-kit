namespace Accounting.Application.Consumptions.Update.v1;

/// <summary>
/// Command to update a consumption record.
/// </summary>
public sealed record UpdateConsumptionCommand(
    DefaultIdType Id,
    string? ReadingType = null,
    string? ReadingSource = null,
    string? Description = null,
    string? Notes = null
) : IRequest<DefaultIdType>;

