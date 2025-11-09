using Accounting.Application.InterCompanyTransactions.Queries;
using Accounting.Application.InterCompanyTransactions.Responses;

namespace Accounting.Application.InterCompanyTransactions.Search.v1;

/// <summary>
/// Handler for searching inter-company transactions with filters and pagination.
/// </summary>
public sealed class SearchInterCompanyTransactionsHandler(
    ILogger<SearchInterCompanyTransactionsHandler> logger,
    [FromKeyedServices("accounting")] IReadRepository<InterCompanyTransaction> repository)
    : IRequestHandler<SearchInterCompanyTransactionsRequest, PagedList<InterCompanyTransactionResponse>>
{
    public async Task<PagedList<InterCompanyTransactionResponse>> Handle(SearchInterCompanyTransactionsRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new InterCompanyTransactionSearchSpec(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Retrieved {Count} of {Total} inter-company transactions", items.Count, totalCount);

        return new PagedList<InterCompanyTransactionResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
