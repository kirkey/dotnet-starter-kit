namespace Accounting.Application.PostingBatches.Get.v1;

/// <summary>
/// Specification for getting a posting batch by ID with all related data.
/// Projects result to <see cref="PostingBatchGetResponse"/>.
/// </summary>
public sealed class PostingBatchByIdSpec : Specification<PostingBatch, PostingBatchGetResponse>, ISingleResultSpecification<PostingBatch>
{
    public PostingBatchByIdSpec(DefaultIdType id)
    {
        Query
            .Where(b => b.Id == id)
            .Include(b => b.JournalEntries);
    }
}

