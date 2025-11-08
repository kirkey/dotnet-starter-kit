namespace Accounting.Application.CostCenters.RecordActual.v1;

public sealed record RecordActualCommand(DefaultIdType Id, decimal Amount) : IRequest<DefaultIdType>;

