using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CashVaults.Reconcile.v1;

public sealed record ReconcileCashVaultCommand(
    DefaultIdType Id,
    decimal PhysicalCount,
    string? DenominationBreakdown = null) : IRequest<ReconcileCashVaultResponse>;
