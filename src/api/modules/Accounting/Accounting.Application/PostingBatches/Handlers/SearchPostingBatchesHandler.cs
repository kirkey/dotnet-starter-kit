using Accounting.Application.PostingBatches.Queries;
using Accounting.Application.PostingBatches.Responses;

namespace Accounting.Application.PostingBatches.Handlers;

/// <summary>
/// Handler for searching posting batches with filtering and pagination.
/// </summary>
public class SearchPostingBatchesHandler(
    [FromKeyedServices("accounting:postingbatches")] IReadRepository<PostingBatch> repository)
    : IRequestHandler<SearchPostingBatchesQuery, PagedList<PostingBatchResponse>>
{
    /// <summary>
    /// Handles the search posting batches query.
    /// </summary>
    /// <param name="request">The search query containing filter criteria and pagination parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paged list of posting batch responses.</returns>
    public async Task<PagedList<PostingBatchResponse>> Handle(SearchPostingBatchesQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchPostingBatchesSpec(request);
        var list = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<PostingBatchResponse>(list, request.PageNumber, request.PageSize, totalCount);
    }
}
