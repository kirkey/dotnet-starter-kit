namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.Create.v1;

public sealed record CreateInvestmentAccountResponse(
    Guid Id,
    string AccountNumber,
    string RiskProfile);
