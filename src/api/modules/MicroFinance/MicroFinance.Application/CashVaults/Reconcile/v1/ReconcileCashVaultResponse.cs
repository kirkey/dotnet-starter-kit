namespace FSH.Starter.WebApi.MicroFinance.Application.CashVaults.Reconcile.v1;

public sealed record ReconcileCashVaultResponse(
    Guid Id,
    decimal ExpectedBalance,
    decimal ActualBalance,
    decimal Variance,
    DateTime ReconciliationDate);
