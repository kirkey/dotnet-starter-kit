namespace Accounting.Application.PostingBatches.Create.v1;

/// <summary>
/// Handler for creating a new posting batch.
/// </summary>
public sealed class PostingBatchCreateHandler(
    IRepository<PostingBatch> repository,
    ILogger<PostingBatchCreateHandler> logger)
    : IRequestHandler<PostingBatchCreateCommand, PostingBatchCreateResponse>
{
    private readonly IRepository<PostingBatch> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<PostingBatchCreateHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<PostingBatchCreateResponse> Handle(PostingBatchCreateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogInformation("Creating posting batch {BatchNumber}", request.BatchNumber);

        // Check if batch number already exists
        var existing = await _repository.ListAsync(cancellationToken);
        if (existing.Any(b => b.BatchNumber.Equals(request.BatchNumber, StringComparison.OrdinalIgnoreCase)))
        {
            _logger.LogWarning("Posting batch number {BatchNumber} already exists", request.BatchNumber);
            throw new InvalidOperationException($"Posting batch number '{request.BatchNumber}' already exists.");
        }

        var batch = PostingBatch.Create(
            request.BatchNumber,
            request.BatchDate,
            request.Description,
            request.PeriodId
        );

        if (!string.IsNullOrWhiteSpace(request.Notes))
        {
            batch.Notes = request.Notes;
        }

        await _repository.AddAsync(batch, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Posting batch {BatchNumber} created successfully with ID {BatchId}",
            batch.BatchNumber, batch.Id);

        return new PostingBatchCreateResponse
        {
            Id = batch.Id,
            BatchNumber = batch.BatchNumber,
            BatchDate = batch.BatchDate,
            Status = batch.Status,
            ApprovalStatus = batch.Status
        };
    }
}
