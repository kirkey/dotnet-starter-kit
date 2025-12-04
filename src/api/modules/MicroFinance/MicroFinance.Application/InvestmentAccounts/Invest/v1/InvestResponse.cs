namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.Invest.v1;

public sealed record InvestResponse(
    Guid Id,
    decimal TotalInvested,
    decimal CurrentValue);
