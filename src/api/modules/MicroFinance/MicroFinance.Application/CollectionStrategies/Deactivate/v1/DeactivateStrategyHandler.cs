using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionStrategies.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionStrategies.Deactivate.v1;

public sealed class DeactivateStrategyHandler(
    [FromKeyedServices("microfinance:collectionstrategies")] IRepository<CollectionStrategy> repository,
    ILogger<DeactivateStrategyHandler> logger)
    : IRequestHandler<DeactivateStrategyCommand, DeactivateStrategyResponse>
{
    public async Task<DeactivateStrategyResponse> Handle(
        DeactivateStrategyCommand request,
        CancellationToken cancellationToken)
    {
        var strategy = await repository.FirstOrDefaultAsync(
            new CollectionStrategyByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Collection strategy {request.Id} not found");

        strategy.Deactivate();
        await repository.UpdateAsync(strategy, cancellationToken);

        logger.LogInformation("Collection strategy deactivated: {StrategyId}", strategy.Id);

        return new DeactivateStrategyResponse(strategy.Id, strategy.IsActive);
    }
}
