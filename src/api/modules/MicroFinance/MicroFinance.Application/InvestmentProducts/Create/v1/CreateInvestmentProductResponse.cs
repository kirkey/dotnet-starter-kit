namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.Create.v1;

public sealed record CreateInvestmentProductResponse(
    DefaultIdType Id,
    string Code,
    string ProductType,
    string RiskLevel);
