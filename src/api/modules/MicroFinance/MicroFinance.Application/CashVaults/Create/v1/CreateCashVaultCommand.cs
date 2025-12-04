using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CashVaults.Create.v1;

public sealed record CreateCashVaultCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] Guid BranchId,
    [property: DefaultValue("VLT-001")] string Code,
    [property: DefaultValue("Main Vault")] string Name,
    [property: DefaultValue("MainVault")] string VaultType,
    [property: DefaultValue(10000)] decimal MinimumBalance,
    [property: DefaultValue(1000000)] decimal MaximumBalance,
    [property: DefaultValue(100000)] decimal OpeningBalance = 0,
    [property: DefaultValue(null)] string? Location = null,
    [property: DefaultValue(null)] string? CustodianName = null,
    [property: DefaultValue(null)] Guid? CustodianUserId = null)
    : IRequest<CreateCashVaultResponse>;
