using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.Get.v1;

public sealed record GetInvestmentProductRequest(Guid Id) : IRequest<InvestmentProductResponse>;
