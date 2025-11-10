namespace Accounting.Application.CostCenters.Delete.v1;

/// <summary>
/// Command to delete a cost center.
/// Only inactive cost centers with no transactions can be deleted.
/// </summary>
public sealed record DeleteCostCenterCommand(DefaultIdType Id) : IRequest<DefaultIdType>;

