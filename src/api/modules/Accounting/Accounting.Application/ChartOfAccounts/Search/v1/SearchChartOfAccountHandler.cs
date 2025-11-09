using Accounting.Application.ChartOfAccounts.Responses;

namespace Accounting.Application.ChartOfAccounts.Search.v1;

/// <summary>
/// Handler for searching chart of accounts.
/// </summary>
public sealed class SearchChartOfAccountHandler(
    [FromKeyedServices("accounting:accounts")] IReadRepository<ChartOfAccount> repository)
    : IRequestHandler<SearchChartOfAccountRequest, PagedList<ChartOfAccountResponse>>
{
    public async Task<PagedList<ChartOfAccountResponse>> Handle(SearchChartOfAccountRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchChartOfAccountSpec(request);

        var list = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<ChartOfAccountResponse>(list, request.PageNumber, request.PageSize, totalCount);
    }
}
