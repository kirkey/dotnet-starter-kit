using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.Activate.v1;

public sealed record ActivateInvestmentProductCommand(
    DefaultIdType Id) : IRequest<ActivateInvestmentProductResponse>;
