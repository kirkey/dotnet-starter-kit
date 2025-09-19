using Accounting.Application.ChartOfAccounts.Responses;

namespace Accounting.Application.ChartOfAccounts.Search.v1;
public sealed class SearchChartOfAccountSpec : EntitiesByPaginationFilterSpec<ChartOfAccount, ChartOfAccountResponse>
{
    public SearchChartOfAccountSpec(SearchChartOfAccountQuery request)
        : base(request)
    {
        Query
            .OrderBy(c => c.AccountCode, !request.HasOrderBy());
    }
}
