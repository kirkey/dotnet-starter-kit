using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.UpdateNav.v1;

public sealed record UpdateInvestmentProductNavCommand(
    Guid Id,
    decimal NewNav,
    DateOnly NavDate) : IRequest<UpdateInvestmentProductNavResponse>;
