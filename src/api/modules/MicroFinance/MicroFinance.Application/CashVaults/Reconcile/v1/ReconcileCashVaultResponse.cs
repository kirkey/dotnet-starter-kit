namespace FSH.Starter.WebApi.MicroFinance.Application.CashVaults.Reconcile.v1;

public sealed record ReconcileCashVaultResponse(
    DefaultIdType Id,
    decimal ExpectedBalance,
    decimal ActualBalance,
    decimal Variance,
    DateTime ReconciliationDate);
