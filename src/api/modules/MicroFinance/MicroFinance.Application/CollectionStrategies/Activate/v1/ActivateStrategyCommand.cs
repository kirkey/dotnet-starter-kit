using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionStrategies.Activate.v1;

public sealed record ActivateStrategyCommand(DefaultIdType Id) : IRequest<ActivateStrategyResponse>;
