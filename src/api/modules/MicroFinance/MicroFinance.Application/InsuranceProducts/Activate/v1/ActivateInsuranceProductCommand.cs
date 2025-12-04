using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsuranceProducts.Activate.v1;

public sealed record ActivateInsuranceProductCommand(Guid Id) : IRequest<ActivateInsuranceProductResponse>;
