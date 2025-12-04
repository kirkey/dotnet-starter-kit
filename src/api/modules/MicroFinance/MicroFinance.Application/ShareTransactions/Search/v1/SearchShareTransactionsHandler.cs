using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.ShareTransactions.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareTransactions.Search.v1;

public sealed class SearchShareTransactionsHandler(
    [FromKeyedServices("microfinance:sharetransactions")] IReadRepository<ShareTransaction> repository)
    : IRequestHandler<SearchShareTransactionsCommand, PagedList<ShareTransactionResponse>>
{
    public async Task<PagedList<ShareTransactionResponse>> Handle(SearchShareTransactionsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchShareTransactionsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<ShareTransactionResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
