using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionStrategies.Get.v1;

public sealed record GetCollectionStrategyRequest(Guid Id) : IRequest<CollectionStrategyResponse>;
