using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Get.v1;

public sealed record GetLoanProductRequest(DefaultIdType Id) : IRequest<LoanProductResponse>;
