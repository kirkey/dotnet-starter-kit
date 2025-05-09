using Accounting.Application.ChartOfAccounts.Dtos;
using Accounting.Domain;
using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;

namespace Accounting.Application.ChartOfAccounts.Search.v1;
public sealed class ChartOfAccountSearchSpec : EntitiesByPaginationFilterSpec<ChartOfAccount, ChartOfAccountDto>
{
    public ChartOfAccountSearchSpec(ChartOfAccountSearchRequest request)
        : base(request)
    {
        Query
            .OrderBy(c => c.AccountCode, !request.HasOrderBy())
            .Where(a => a.AccountCode.Contains(request.Keyword!)
                || a.Name.Contains(request.Keyword!)
                || a.Description!.Contains(request.Keyword!)
                || a.Notes!.Contains(request.Keyword!),
                !string.IsNullOrEmpty(request.Keyword));
    }
}
