namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.Create.v1;

public sealed record CreateInvestmentProductResponse(
    Guid Id,
    string Code,
    string ProductType,
    string RiskLevel);
