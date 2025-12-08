using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsuranceProducts.Get.v1;

public sealed record GetInsuranceProductRequest(DefaultIdType Id) : IRequest<InsuranceProductResponse>;
