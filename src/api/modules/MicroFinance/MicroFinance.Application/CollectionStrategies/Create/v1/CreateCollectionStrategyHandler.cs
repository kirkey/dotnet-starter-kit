using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionStrategies.Create.v1;

public sealed class CreateCollectionStrategyHandler(
    [FromKeyedServices("microfinance:collectionstrategies")] IRepository<CollectionStrategy> repository,
    ILogger<CreateCollectionStrategyHandler> logger)
    : IRequestHandler<CreateCollectionStrategyCommand, CreateCollectionStrategyResponse>
{
    public async Task<CreateCollectionStrategyResponse> Handle(
        CreateCollectionStrategyCommand request,
        CancellationToken cancellationToken)
    {
        var strategy = CollectionStrategy.Create(
            request.Code,
            request.Name,
            request.TriggerDaysPastDue,
            request.ActionType,
            request.Priority);

        await repository.AddAsync(strategy, cancellationToken);

        logger.LogInformation("Collection strategy created: {StrategyId} with code {Code}",
            strategy.Id, request.Code);

        return new CreateCollectionStrategyResponse(strategy.Id);
    }
}
