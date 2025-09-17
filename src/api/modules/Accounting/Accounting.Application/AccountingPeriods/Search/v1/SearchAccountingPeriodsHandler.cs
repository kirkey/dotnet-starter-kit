using Accounting.Application.AccountingPeriods.Dtos;

namespace Accounting.Application.AccountingPeriods.Search.v1;

public sealed class SearchAccountingPeriodsHandler(
    [FromKeyedServices("accounting:periods")] IReadRepository<AccountingPeriod> repository)
    : IRequestHandler<SearchAccountingPeriodsRequest, PagedList<AccountingPeriodDto>>
{
    public async Task<PagedList<AccountingPeriodDto>> Handle(SearchAccountingPeriodsRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchAccountingPeriodsSpec(request);
        var list = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<AccountingPeriodDto>(list, request.PageNumber, request.PageSize, totalCount);
    }
}


