using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeDefinitions.Get.v1;

public sealed record GetFeeDefinitionRequest(DefaultIdType Id) : IRequest<FeeDefinitionResponse>;
