using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionStrategies.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionStrategies.Update.v1;

public sealed class UpdateCollectionStrategyHandler(
    [FromKeyedServices("microfinance:collectionstrategies")] IRepository<CollectionStrategy> repository,
    ILogger<UpdateCollectionStrategyHandler> logger)
    : IRequestHandler<UpdateCollectionStrategyCommand, UpdateCollectionStrategyResponse>
{
    public async Task<UpdateCollectionStrategyResponse> Handle(
        UpdateCollectionStrategyCommand request,
        CancellationToken cancellationToken)
    {
        var strategy = await repository.FirstOrDefaultAsync(
            new CollectionStrategyByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (strategy is null)
        {
            throw new NotFoundException($"Collection strategy with ID {request.Id} not found.");
        }

        strategy.Update(
            request.Name,
            request.Description,
            request.TriggerDaysPastDue,
            request.MaxDaysPastDue,
            request.ActionType,
            request.MessageTemplate,
            request.Priority);

        await repository.UpdateAsync(strategy, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Collection strategy updated: {StrategyId}", request.Id);

        return new UpdateCollectionStrategyResponse(strategy.Id);
    }
}
