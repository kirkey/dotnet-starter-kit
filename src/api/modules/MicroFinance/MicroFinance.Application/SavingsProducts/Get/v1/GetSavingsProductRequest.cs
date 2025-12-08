using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsProducts.Get.v1;

public sealed record GetSavingsProductRequest(DefaultIdType Id) : IRequest<SavingsProductResponse>;
