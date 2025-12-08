using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionStrategies.Update.v1;

public sealed record UpdateCollectionStrategyCommand(
    DefaultIdType Id,
    string? Name,
    string? Description,
    int? TriggerDaysPastDue,
    int? MaxDaysPastDue,
    string? ActionType,
    string? MessageTemplate,
    int? Priority) : IRequest<UpdateCollectionStrategyResponse>;
