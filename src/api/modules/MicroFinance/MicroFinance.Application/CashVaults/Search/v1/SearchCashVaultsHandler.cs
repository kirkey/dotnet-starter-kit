using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.CashVaults.Search.v1;

public sealed class SearchCashVaultsHandler(
    [FromKeyedServices("microfinance:cashvaults")] IReadRepository<CashVault> repository)
    : IRequestHandler<SearchCashVaultsCommand, PagedList<CashVaultSummaryResponse>>
{
    public async Task<PagedList<CashVaultSummaryResponse>> Handle(
        SearchCashVaultsCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchCashVaultsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<CashVaultSummaryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
