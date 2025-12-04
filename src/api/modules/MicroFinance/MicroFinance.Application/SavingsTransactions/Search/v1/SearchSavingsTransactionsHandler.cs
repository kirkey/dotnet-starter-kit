using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsTransactions.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsTransactions.Search.v1;

public sealed class SearchSavingsTransactionsHandler(
    [FromKeyedServices("microfinance:savingstransactions")] IReadRepository<SavingsTransaction> repository)
    : IRequestHandler<SearchSavingsTransactionsCommand, PagedList<SavingsTransactionResponse>>
{
    public async Task<PagedList<SavingsTransactionResponse>> Handle(SearchSavingsTransactionsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchSavingsTransactionsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<SavingsTransactionResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
