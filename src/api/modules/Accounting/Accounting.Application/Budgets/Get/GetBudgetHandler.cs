using Accounting.Application.Budgets.Responses;

namespace Accounting.Application.Budgets.Get;

public sealed class GetBudgetHandler(
    [FromKeyedServices("accounting:budgets")] IReadRepository<Budget> repository,
    ICacheService cache)
    : IRequestHandler<GetBudgetQuery, BudgetResponse>
{
    public async Task<BudgetResponse> Handle(GetBudgetQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var item = await cache.GetOrSetAsync(
            $"budget:{request.Id}",
            async () =>
            {
                var budget = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
                if (budget == null) throw new BudgetNotFoundException(request.Id);
                return new BudgetResponse(
                    budget.Id,
                    budget.Name,
                    budget.PeriodId,
                    budget.FiscalYear,
                    budget.BudgetType,
                    budget.Status,
                    budget.TotalBudgetedAmount,
                    budget.TotalActualAmount,
                    budget.ApprovedDate,
                    budget.ApprovedBy,
                    budget.Description,
                    budget.Notes);
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return item!;
    }
}
