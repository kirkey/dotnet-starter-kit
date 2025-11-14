using MediatR;

namespace FSH.Starter.WebApi.HumanResources.Application.PayComponentRates.Get.v1;

public sealed record GetPayComponentRateRequest(DefaultIdType Id) : IRequest<PayComponentRateResponse>;

