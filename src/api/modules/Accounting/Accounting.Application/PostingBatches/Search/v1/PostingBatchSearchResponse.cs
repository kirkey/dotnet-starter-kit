namespace Accounting.Application.PostingBatches.Search.v1;

/// <summary>
/// Handler for searching posting batches.
/// </summary>
public sealed class PostingBatchSearchHandler(
    [FromKeyedServices("accounting:posting-batches")] IReadRepository<PostingBatch> repository)
    : IRequestHandler<PostingBatchSearchQuery, PagedList<PostingBatchSearchResponse>>
{
    public async Task<PagedList<PostingBatchSearchResponse>> Handle(PostingBatchSearchQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new PostingBatchSearchSpec(request);
        var list = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<PostingBatchSearchResponse>(list, request.PageNumber, request.PageSize, totalCount);
    }
}
