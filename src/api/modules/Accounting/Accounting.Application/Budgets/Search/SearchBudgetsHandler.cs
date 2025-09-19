using Accounting.Application.Budgets.Responses;

namespace Accounting.Application.Budgets.Search;

public sealed class SearchBudgetsHandler(
    [FromKeyedServices("accounting:budgets")] IReadRepository<Budget> repository)
    : IRequestHandler<SearchBudgetsQuery, PagedList<BudgetListItemResponse>>
{
    public async Task<PagedList<BudgetListItemResponse>> Handle(SearchBudgetsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchBudgetsSpec(request);
        var list = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<BudgetListItemResponse>(list, request.PageNumber, request.PageSize, totalCount);
    }
}
