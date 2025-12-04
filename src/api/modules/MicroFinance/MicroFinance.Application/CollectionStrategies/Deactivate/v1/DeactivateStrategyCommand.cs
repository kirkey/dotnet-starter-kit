using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionStrategies.Deactivate.v1;

public sealed record DeactivateStrategyCommand(Guid Id) : IRequest<DeactivateStrategyResponse>;
