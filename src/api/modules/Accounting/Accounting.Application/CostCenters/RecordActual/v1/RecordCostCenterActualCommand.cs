namespace Accounting.Application.CostCenters.RecordActual.v1;

/// <summary>
/// Command to record actual spending amount for a cost center.
/// </summary>
public sealed record RecordCostCenterActualCommand(DefaultIdType Id, decimal Amount) : IRequest<DefaultIdType>;

