using Accounting.Application.Budgets.Details.Responses;
using Accounting.Application.Budgets.Details.Specs;

namespace Accounting.Application.Budgets.Details.Search;

/// <summary>
/// Handler for searching budget details by budget ID.
/// Uses database-level projection for optimal performance.
/// </summary>
public sealed class SearchBudgetDetailsByBudgetIdHandler(IReadRepository<BudgetDetail> repo)
    : IRequestHandler<SearchBudgetDetailsByBudgetIdQuery, List<BudgetDetailResponse>>
{
    /// <summary>
    /// Handles the search query to retrieve budget details for a specific budget.
    /// </summary>
    /// <param name="request">The search query containing the budget ID.</param>
    /// <param name="ct">Cancellation token for async operations.</param>
    /// <returns>List of budget detail responses.</returns>
    public async Task<List<BudgetDetailResponse>> Handle(SearchBudgetDetailsByBudgetIdQuery request, CancellationToken ct)
    {
        var spec = new GetBudgetDetailsSpec(request.BudgetId);
        var items = await repo.ListAsync(spec, ct).ConfigureAwait(false);
        return items.ToList();
    }
}

