using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionStrategies.Get.v1;

public sealed class GetCollectionStrategyHandler(
    [FromKeyedServices("microfinance:collectionstrategies")] IReadRepository<CollectionStrategy> repository)
    : IRequestHandler<GetCollectionStrategyRequest, CollectionStrategyResponse>
{
    public async Task<CollectionStrategyResponse> Handle(
        GetCollectionStrategyRequest request,
        CancellationToken cancellationToken)
    {
        var strategy = await repository.FirstOrDefaultAsync(
            new CollectionStrategyByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Collection strategy {request.Id} not found");

        return new CollectionStrategyResponse(
            strategy.Id,
            strategy.Code,
            strategy.Name,
            strategy.Description,
            strategy.LoanProductId,
            strategy.TriggerDaysPastDue,
            strategy.MaxDaysPastDue,
            strategy.MinOutstandingAmount,
            strategy.MaxOutstandingAmount,
            strategy.ActionType,
            strategy.MessageTemplate,
            strategy.Priority,
            strategy.RepeatIntervalDays,
            strategy.MaxRepetitions,
            strategy.EscalateOnFailure,
            strategy.RequiresApproval,
            strategy.IsActive,
            strategy.EffectiveFrom,
            strategy.EffectiveTo);
    }
}
