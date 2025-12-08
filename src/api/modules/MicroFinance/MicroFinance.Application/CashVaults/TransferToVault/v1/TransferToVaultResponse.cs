namespace FSH.Starter.WebApi.MicroFinance.Application.CashVaults.TransferToVault.v1;

/// <summary>
/// Response after transferring cash between vaults.
/// </summary>
public sealed record TransferToVaultResponse(
    DefaultIdType SourceVaultId, 
    DefaultIdType TargetVaultId, 
    decimal Amount,
    decimal SourceNewBalance,
    decimal TargetNewBalance,
    string Message);
