using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionStrategies.Create.v1;

public sealed record CreateCollectionStrategyCommand(
    string Code,
    string Name,
    int TriggerDaysPastDue,
    string ActionType,
    int Priority) : IRequest<CreateCollectionStrategyResponse>;
