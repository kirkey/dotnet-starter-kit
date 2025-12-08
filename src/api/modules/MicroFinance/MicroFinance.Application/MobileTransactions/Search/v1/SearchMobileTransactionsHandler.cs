// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/MicroFinance/MicroFinance.Application/MobileTransactions/Search/v1/SearchMobileTransactionsHandler.cs
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.MobileTransactions.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileTransactions.Search.v1;

/// <summary>
/// Handler for searching mobile transactions.
/// </summary>
public sealed class SearchMobileTransactionsHandler(
    [FromKeyedServices("microfinance:mobiletransactions")] IReadRepository<MobileTransaction> repository)
    : IRequestHandler<SearchMobileTransactionsCommand, PagedList<MobileTransactionResponse>>
{
    public async Task<PagedList<MobileTransactionResponse>> Handle(SearchMobileTransactionsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchMobileTransactionsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<MobileTransactionResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}

