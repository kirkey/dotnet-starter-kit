namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.Redeem.v1;

public sealed record RedeemResponse(
    DefaultIdType Id,
    decimal CurrentValue,
    decimal RealizedGains);
