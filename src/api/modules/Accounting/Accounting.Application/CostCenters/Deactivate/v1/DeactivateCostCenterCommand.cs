namespace Accounting.Application.CostCenters.Deactivate.v1;

public sealed record DeactivateCostCenterCommand(DefaultIdType Id) : IRequest<DefaultIdType>;

