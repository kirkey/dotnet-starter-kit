using Accounting.Application.Budgets.Details.Responses;
using Accounting.Domain.Entities;

namespace Accounting.Application.Budgets.Details.Search;

public sealed class SearchBudgetDetailsByBudgetIdHandler(IReadRepository<BudgetDetail> repo)
    : IRequestHandler<SearchBudgetDetailsByBudgetIdQuery, List<BudgetDetailResponse>>
{
    public async Task<List<BudgetDetailResponse>> Handle(SearchBudgetDetailsByBudgetIdQuery request, CancellationToken ct)
    {
        var items = await repo.ListAsync(new Specs.BudgetDetailsByBudgetIdSpec(request.BudgetId), ct);
        return items.Select(d => new BudgetDetailResponse(d.Id, d.BudgetId, d.AccountId, d.BudgetedAmount, d.ActualAmount, d.Description)).ToList();
    }
}

