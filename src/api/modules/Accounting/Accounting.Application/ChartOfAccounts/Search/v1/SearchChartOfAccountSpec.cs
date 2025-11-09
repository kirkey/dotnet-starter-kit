using Accounting.Application.ChartOfAccounts.Responses;

namespace Accounting.Application.ChartOfAccounts.Search.v1;

/// <summary>
/// Specification for searching chart of accounts.
/// </summary>
public sealed class SearchChartOfAccountSpec : EntitiesByPaginationFilterSpec<ChartOfAccount, ChartOfAccountResponse>
{
    public SearchChartOfAccountSpec(SearchChartOfAccountRequest request)
        : base(request)
    {
        Query
            .OrderBy(c => c.AccountCode, !request.HasOrderBy());
    }
}
