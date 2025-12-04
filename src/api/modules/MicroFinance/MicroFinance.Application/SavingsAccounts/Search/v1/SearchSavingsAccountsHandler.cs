using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Search.v1;

/// <summary>
/// Handler for searching savings accounts.
/// </summary>
public sealed class SearchSavingsAccountsHandler(
    [FromKeyedServices("microfinance:savingsaccounts")] IReadRepository<SavingsAccount> repository)
    : IRequestHandler<SearchSavingsAccountsCommand, PagedList<SavingsAccountResponse>>
{
    public async Task<PagedList<SavingsAccountResponse>> Handle(SearchSavingsAccountsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchSavingsAccountsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<SavingsAccountResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
