namespace FSH.Starter.WebApi.Store.Application.CycleCounts.Reconcile.v1;

public sealed record ReconcileCycleCountResponse(DefaultIdType CycleCountId, IReadOnlyList<CycleCountDiscrepancy> Discrepancies);

