namespace Accounting.Application.Consumptions.Delete.v1;

/// <summary>
/// Command to delete a consumption record.
/// </summary>
public sealed record DeleteConsumptionCommand(DefaultIdType Id) : IRequest<DefaultIdType>;

