using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CashVaults.CloseDay.v1;

/// <summary>
/// Command to close a cash vault for the business day.
/// </summary>
public sealed record CloseDayCashVaultCommand(
    DefaultIdType CashVaultId,
    decimal VerifiedBalance,
    string? DenominationBreakdown = null) : IRequest<CloseDayCashVaultResponse>;
