using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionStrategies.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionStrategies.Activate.v1;

public sealed class ActivateStrategyHandler(
    [FromKeyedServices("microfinance:collectionstrategies")] IRepository<CollectionStrategy> repository,
    ILogger<ActivateStrategyHandler> logger)
    : IRequestHandler<ActivateStrategyCommand, ActivateStrategyResponse>
{
    public async Task<ActivateStrategyResponse> Handle(
        ActivateStrategyCommand request,
        CancellationToken cancellationToken)
    {
        var strategy = await repository.FirstOrDefaultAsync(
            new CollectionStrategyByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Collection strategy {request.Id} not found");

        strategy.Activate();
        await repository.UpdateAsync(strategy, cancellationToken);

        logger.LogInformation("Collection strategy activated: {StrategyId}", strategy.Id);

        return new ActivateStrategyResponse(strategy.Id, strategy.IsActive);
    }
}
