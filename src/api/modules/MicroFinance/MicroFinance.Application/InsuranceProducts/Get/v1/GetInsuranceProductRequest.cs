using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsuranceProducts.Get.v1;

public sealed record GetInsuranceProductRequest(Guid Id) : IRequest<InsuranceProductResponse>;
