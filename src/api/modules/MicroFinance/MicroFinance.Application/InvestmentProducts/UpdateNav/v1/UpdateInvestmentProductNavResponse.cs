namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.UpdateNav.v1;

public sealed record UpdateInvestmentProductNavResponse(
    Guid Id,
    decimal CurrentNav,
    DateOnly NavDate);
