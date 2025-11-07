namespace Accounting.Application.PostingBatches.Search.v1;

public sealed class PostingBatchSearchHandler(
    IReadRepository<PostingBatch> repository,
    ILogger<PostingBatchSearchHandler> logger)
    : IRequestHandler<PostingBatchSearchQuery, PagedList<PostingBatchSearchResponse>>
{
    private readonly IReadRepository<PostingBatch> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<PostingBatchSearchHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<PagedList<PostingBatchSearchResponse>> Handle(PostingBatchSearchQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogInformation("Searching posting batches with filters");

        var spec = new PostingBatchSearchSpec(request);
        var batches = await _repository.ListAsync(spec, cancellationToken);
        var totalCount = await _repository.CountAsync(cancellationToken);

        var response = batches.Select(b => new PostingBatchSearchResponse
        {
            Id = b.Id,
            BatchNumber = b.BatchNumber,
            BatchDate = b.BatchDate,
            Status = b.Status,
            ApprovalStatus = b.ApprovalStatus,
            Description = b.Description,
            JournalEntryCount = b.JournalEntries.Count,
            CreatedOn = b.CreatedOn.DateTime
        }).ToList();

        _logger.LogInformation("Found {Count} posting batches", response.Count);

        return new PagedList<PostingBatchSearchResponse>(
            response,
            totalCount,
            request.PageNumber,
            request.PageSize);
    }
}
