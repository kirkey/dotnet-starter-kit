namespace Accounting.Application.PostingBatches.Get.v1;

/// <summary>
/// Handler for retrieving a posting batch by ID.
/// </summary>
public sealed class PostingBatchGetHandler(
    IReadRepository<PostingBatch> repository,
    ILogger<PostingBatchGetHandler> logger)
    : IRequestHandler<PostingBatchGetQuery, PostingBatchGetResponse>
{
    private readonly IReadRepository<PostingBatch> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<PostingBatchGetHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<PostingBatchGetResponse> Handle(PostingBatchGetQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogInformation("Retrieving posting batch with ID {BatchId}", request.Id);

        var batch = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (batch == null)
        {
            _logger.LogWarning("Posting batch with ID {BatchId} not found", request.Id);
            throw new NotFoundException($"Posting batch with ID {request.Id} not found");
        }

        _logger.LogInformation("Posting batch {BatchNumber} retrieved successfully", batch.BatchNumber);

        return new PostingBatchGetResponse
        {
            Id = batch.Id,
            BatchNumber = batch.BatchNumber,
            BatchDate = batch.BatchDate,
            Status = batch.Status,
            PeriodId = batch.PeriodId,
            ApprovalStatus = batch.Status,
            ApprovedBy = batch.ApproverName,
            ApprovedDate = batch.ApprovedOn,
            Description = batch.Description,
            Notes = batch.Notes,
            JournalEntryCount = batch.JournalEntries.Count,
            CreatedOn = batch.CreatedOn.DateTime,
            CreatedByUserName = batch.CreatedByUserName
        };
    }
}
