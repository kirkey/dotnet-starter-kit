using Accounting.Application.Budgets.Dtos;

namespace Accounting.Application.Budgets.Get;

public sealed class GetBudgetHandler(
    [FromKeyedServices("accounting:budgets")] IReadRepository<Budget> repository,
    ICacheService cache)
    : IRequestHandler<GetBudgetRequest, BudgetDto>
{
    public async Task<BudgetDto> Handle(GetBudgetRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var item = await cache.GetOrSetAsync(
            $"budget:{request.Id}",
            async () =>
            {
                var budget = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
                if (budget == null) throw new BudgetNotFoundException(request.Id);
                return budget.Adapt<BudgetDto>();
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return item!;
    }
}
