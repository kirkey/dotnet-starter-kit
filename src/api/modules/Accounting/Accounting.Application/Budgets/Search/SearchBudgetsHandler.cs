using Accounting.Application.Budgets.Dtos;

namespace Accounting.Application.Budgets.Search;

public sealed class SearchBudgetsHandler(
    [FromKeyedServices("accounting:budgets")] IReadRepository<Budget> repository)
    : IRequestHandler<SearchBudgetsRequest, PagedList<BudgetDto>>
{
    public async Task<PagedList<BudgetDto>> Handle(SearchBudgetsRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchBudgetsSpec(request);
        var list = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<BudgetDto>(list, request.PageNumber, request.PageSize, totalCount);
    }
}


