namespace Accounting.Application.PostingBatches.Get.v1;

/// <summary>
/// Handler for retrieving a posting batch by ID.
/// </summary>
public sealed class PostingBatchGetHandler(
    [FromKeyedServices("accounting:postingBatches")] IReadRepository<PostingBatch> repository)
    : IRequestHandler<PostingBatchGetQuery, PostingBatchGetResponse>
{
    public async Task<PostingBatchGetResponse> Handle(PostingBatchGetQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new PostingBatchByIdSpec(request.Id);
        var batch = await repository.FirstOrDefaultAsync(spec, cancellationToken);
        
        if (batch == null)
            throw new NotFoundException($"Posting batch with ID {request.Id} not found");

        return batch;
    }
}
