namespace Accounting.Application.CostCenters.Activate.v1;

public sealed record ActivateCostCenterCommand(DefaultIdType Id) : IRequest<DefaultIdType>;

