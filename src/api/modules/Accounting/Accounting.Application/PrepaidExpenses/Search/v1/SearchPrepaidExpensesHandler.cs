using Accounting.Application.PrepaidExpenses.Responses;

namespace Accounting.Application.PrepaidExpenses.Search.v1;

/// <summary>
/// Handler for searching prepaid expenses with filters and pagination.
/// </summary>
public sealed class SearchPrepaidExpensesHandler(
    ILogger<SearchPrepaidExpensesHandler> logger,
    [FromKeyedServices("accounting")] IReadRepository<PrepaidExpense> repository)
    : IRequestHandler<SearchPrepaidExpensesRequest, PagedList<PrepaidExpenseResponse>>
{
    public async Task<PagedList<PrepaidExpenseResponse>> Handle(SearchPrepaidExpensesRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchPrepaidExpensesSpec(request);
        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Retrieved {Count} of {Total} prepaid expenses", items.Count, totalCount);

        return new PagedList<PrepaidExpenseResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}

