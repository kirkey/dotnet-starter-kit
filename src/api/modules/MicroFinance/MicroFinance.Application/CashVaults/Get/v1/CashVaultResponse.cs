namespace FSH.Starter.WebApi.MicroFinance.Application.CashVaults.Get.v1;

public sealed record CashVaultResponse(
    DefaultIdType Id,
    DefaultIdType BranchId,
    string Code,
    string Name,
    string VaultType,
    string Status,
    decimal CurrentBalance,
    decimal OpeningBalance,
    decimal MinimumBalance,
    decimal MaximumBalance,
    string? Location,
    string? CustodianName,
    DefaultIdType? CustodianUserId,
    DateTime? LastReconciliationDate,
    decimal? LastReconciledBalance,
    string? Notes);
