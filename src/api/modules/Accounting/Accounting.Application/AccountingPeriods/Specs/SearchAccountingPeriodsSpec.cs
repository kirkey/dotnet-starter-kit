using Accounting.Application.AccountingPeriods.Responses;
using Accounting.Application.AccountingPeriods.Search.v1;

namespace Accounting.Application.AccountingPeriods.Specs;

/// <summary>
/// Specification used to apply search filters and paging to AccountingPeriod queries.
/// Projects domain entities to <see cref="AccountingPeriodResponse"/> for the read model.
/// </summary>
public sealed class SearchAccountingPeriodsSpec : EntitiesByPaginationFilterSpec<AccountingPeriod, AccountingPeriodResponse>
{
    public SearchAccountingPeriodsSpec(SearchAccountingPeriodsRequest request) : base(request)
    {
        Query
            .OrderBy(p => p.Name!, !request.HasOrderBy())
            .Where(p => p.Name!.Contains(request.Name!), !string.IsNullOrEmpty(request.Name))
            .Where(p => p.FiscalYear == request.FiscalYear, request.FiscalYear.HasValue)
            .Where(p => p.IsClosed == request.IsClosed);
    }
}
