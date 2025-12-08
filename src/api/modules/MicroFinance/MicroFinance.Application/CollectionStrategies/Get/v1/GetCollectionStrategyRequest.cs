using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionStrategies.Get.v1;

public sealed record GetCollectionStrategyRequest(DefaultIdType Id) : IRequest<CollectionStrategyResponse>;
