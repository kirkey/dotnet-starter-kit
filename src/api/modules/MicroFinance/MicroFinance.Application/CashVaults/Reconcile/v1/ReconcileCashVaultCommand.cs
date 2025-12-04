using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CashVaults.Reconcile.v1;

public sealed record ReconcileCashVaultCommand(
    Guid Id,
    decimal PhysicalCount,
    string? DenominationBreakdown = null) : IRequest<ReconcileCashVaultResponse>;
