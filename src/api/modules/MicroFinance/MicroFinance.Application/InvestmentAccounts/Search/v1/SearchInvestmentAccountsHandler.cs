using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.Search.v1;

/// <summary>
/// Handler for searching investment accounts.
/// </summary>
public sealed class SearchInvestmentAccountsHandler(
    [FromKeyedServices("microfinance:investmentaccounts")] IReadRepository<InvestmentAccount> repository)
    : IRequestHandler<SearchInvestmentAccountsCommand, PagedList<InvestmentAccountResponse>>
{
    public async Task<PagedList<InvestmentAccountResponse>> Handle(SearchInvestmentAccountsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchInvestmentAccountsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<InvestmentAccountResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}

