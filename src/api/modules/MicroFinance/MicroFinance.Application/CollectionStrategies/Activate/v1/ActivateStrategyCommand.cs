using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionStrategies.Activate.v1;

public sealed record ActivateStrategyCommand(Guid Id) : IRequest<ActivateStrategyResponse>;
