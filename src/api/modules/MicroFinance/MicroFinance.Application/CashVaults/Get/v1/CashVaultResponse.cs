namespace FSH.Starter.WebApi.MicroFinance.Application.CashVaults.Get.v1;

public sealed record CashVaultResponse(
    Guid Id,
    Guid BranchId,
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
    Guid? CustodianUserId,
    DateTime? LastReconciliationDate,
    decimal? LastReconciledBalance,
    string? Notes);
