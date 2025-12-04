namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.Redeem.v1;

public sealed record RedeemResponse(
    Guid Id,
    decimal CurrentValue,
    decimal RealizedGains);
