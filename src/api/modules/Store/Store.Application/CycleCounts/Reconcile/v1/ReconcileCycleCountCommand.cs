namespace FSH.Starter.WebApi.Store.Application.CycleCounts.Reconcile.v1;

public sealed record CycleCountDiscrepancy(DefaultIdType ItemId, int SystemQuantity, int CountedQuantity, int Difference);

public sealed record ReconcileCycleCountCommand(DefaultIdType Id) : IRequest<ReconcileCycleCountResponse>;
