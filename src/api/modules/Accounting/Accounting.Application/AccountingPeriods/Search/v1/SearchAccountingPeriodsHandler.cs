using Accounting.Application.AccountingPeriods.Responses;
using Accounting.Application.AccountingPeriods.Specs;

namespace Accounting.Application.AccountingPeriods.Search.v1;

public sealed class SearchAccountingPeriodsHandler(
    [FromKeyedServices("accounting:periods")] IReadRepository<AccountingPeriod> repository)
    : IRequestHandler<SearchAccountingPeriodsQuery, PagedList<AccountingPeriodResponse>>
{
    public async Task<PagedList<AccountingPeriodResponse>> Handle(SearchAccountingPeriodsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchAccountingPeriodsSpec(request);
        var list = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<AccountingPeriodResponse>(list, request.PageNumber, request.PageSize, totalCount);
    }
}
