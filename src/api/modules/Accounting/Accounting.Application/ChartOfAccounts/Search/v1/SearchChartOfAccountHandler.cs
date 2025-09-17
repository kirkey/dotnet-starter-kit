using Accounting.Application.ChartOfAccounts.Dtos;

namespace Accounting.Application.ChartOfAccounts.Search.v1;

public sealed class SearchChartOfAccountHandler(
    [FromKeyedServices("accounting:accounts")] IReadRepository<ChartOfAccount> repository)
    : IRequestHandler<SearchChartOfAccountRequest, PagedList<ChartOfAccountDto>>
{
    public async Task<PagedList<ChartOfAccountDto>> Handle(SearchChartOfAccountRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchChartOfAccountSpec(request);

        var list = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<ChartOfAccountDto>(list, request.PageNumber, request.PageSize, totalCount);
    }
}
