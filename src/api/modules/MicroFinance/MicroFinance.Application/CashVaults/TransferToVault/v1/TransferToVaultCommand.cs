using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CashVaults.TransferToVault.v1;

/// <summary>
/// Command to transfer cash between vaults.
/// </summary>
public sealed record TransferToVaultCommand(
    DefaultIdType SourceVaultId,
    DefaultIdType TargetVaultId,
    decimal Amount,
    string? DenominationBreakdown = null) : IRequest<TransferToVaultResponse>;
