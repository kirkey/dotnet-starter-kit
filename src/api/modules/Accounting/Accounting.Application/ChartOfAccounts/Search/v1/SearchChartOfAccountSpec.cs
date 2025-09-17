using Accounting.Application.ChartOfAccounts.Dtos;

namespace Accounting.Application.ChartOfAccounts.Search.v1;
public sealed class SearchChartOfAccountSpec : EntitiesByPaginationFilterSpec<ChartOfAccount, ChartOfAccountDto>
{
    public SearchChartOfAccountSpec(SearchChartOfAccountRequest request)
        : base(request)
    {
        Query
            .OrderBy(c => c.AccountCode, !request.HasOrderBy());
        // .Where(a => a.AccountCode.Contains(request.Keyword!)
        //     || a.Name.Contains(request.Keyword!)
        //     || a.Description!.Contains(request.Keyword!)
        //     || a.Notes!.Contains(request.Keyword!),
        //     !string.IsNullOrEmpty(request.Keyword));
    }
}
