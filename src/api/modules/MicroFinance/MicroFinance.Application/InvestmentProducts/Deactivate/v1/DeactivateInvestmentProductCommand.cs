using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.Deactivate.v1;

public sealed record DeactivateInvestmentProductCommand(
    DefaultIdType Id) : IRequest<DeactivateInvestmentProductResponse>;
