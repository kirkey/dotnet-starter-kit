using Accounting.Application.AccountingPeriods.Dtos;

namespace Accounting.Application.AccountingPeriods.Search;

public sealed class SearchAccountingPeriodsSpec : EntitiesByPaginationFilterSpec<AccountingPeriod, AccountingPeriodDto>
{
    public SearchAccountingPeriodsSpec(SearchAccountingPeriodsRequest request) : base(request)
    {
        Query
            .OrderBy(p => p.Name!, !request.HasOrderBy())
            .Where(p => p.Name!.Contains(request.Name!), !string.IsNullOrEmpty(request.Name))
            .Where(p => p.FiscalYear == request.FiscalYear, request.FiscalYear.HasValue)
            .Where(p => p.IsClosed == request.IsClosed, request.IsClosed.HasValue);
    }
}


