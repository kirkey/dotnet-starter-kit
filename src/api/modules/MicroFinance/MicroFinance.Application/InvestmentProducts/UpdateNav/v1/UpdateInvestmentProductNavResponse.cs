namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.UpdateNav.v1;

public sealed record UpdateInvestmentProductNavResponse(
    DefaultIdType Id,
    decimal CurrentNav,
    DateOnly NavDate);
