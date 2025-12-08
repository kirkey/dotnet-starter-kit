using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Framework.Core.Specifications;
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

        var spec = new EntitiesByPaginationFilterSpec<InvestmentAccount, InvestmentAccountResponse>(
            new PaginationFilter
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Keyword = request.Keyword,
                OrderBy = request.OrderBy
            });

        return await repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken)
            .ConfigureAwait(false);
    }
}

