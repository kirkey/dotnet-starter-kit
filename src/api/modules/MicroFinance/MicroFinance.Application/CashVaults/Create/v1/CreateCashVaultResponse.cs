namespace FSH.Starter.WebApi.MicroFinance.Application.CashVaults.Create.v1;

public sealed record CreateCashVaultResponse(DefaultIdType Id, string Code, string Name, decimal CurrentBalance);
