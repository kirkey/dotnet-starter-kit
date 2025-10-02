using Accounting.Application.Budgets.Responses;
using Accounting.Domain.Entities;

namespace Accounting.Application.Budgets.Get;

public sealed class GetBudgetHandler(
    [FromKeyedServices("accounting:budgets")] IReadRepository<Budget> repository,
    ICacheService cache)
    : IRequestHandler<GetBudgetQuery, BudgetResponse>
{
    public async Task<BudgetResponse> Handle(GetBudgetQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response = await cache.GetOrSetAsync(
            $"budget:{request.Id}",
            async () =>
            {
                var budget = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
                if (budget == null) throw new BudgetNotFoundException(request.Id);
                return budget.Adapt<BudgetResponse>();
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return response!;
    }
}
