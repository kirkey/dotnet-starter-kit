using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentTransactions.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentTransactions.Search.v1;

/// <summary>
/// Handler for searching investment transactions.
/// </summary>
public sealed class SearchInvestmentTransactionsHandler(
    [FromKeyedServices("microfinance:investmenttransactions")] IReadRepository<InvestmentTransaction> repository)
    : IRequestHandler<SearchInvestmentTransactionsCommand, PagedList<InvestmentTransactionResponse>>
{
    public async Task<PagedList<InvestmentTransactionResponse>> Handle(SearchInvestmentTransactionsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new EntitiesByPaginationFilterSpec<InvestmentTransaction, InvestmentTransactionResponse>(
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

