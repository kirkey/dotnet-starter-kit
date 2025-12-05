namespace Accounting.Application.PostingBatches.Create.v1;

/// <summary>
/// Handler for creating a new posting batch.
/// </summary>
public sealed class CreatePostingBatchHandler(
    [FromKeyedServices("accounting:posting-batches")] IRepository<PostingBatch> repository,
    ILogger<CreatePostingBatchHandler> logger)
    : IRequestHandler<CreatePostingBatchCommand, PostingBatchCreateResponse>
{
    public async Task<PostingBatchCreateResponse> Handle(CreatePostingBatchCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("Creating posting batch {BatchNumber}", request.BatchNumber);

        // Check if batch number already exists
        var existing = await repository.ListAsync(cancellationToken);
        if (existing.Any(b => b.BatchNumber.Equals(request.BatchNumber, StringComparison.OrdinalIgnoreCase)))
        {
            logger.LogWarning("Posting batch number {BatchNumber} already exists", request.BatchNumber);
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

        await repository.AddAsync(batch, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Posting batch {BatchNumber} created successfully with ID {BatchId}",
            batch.BatchNumber, batch.Id);

        return new PostingBatchCreateResponse
        {
            Id = batch.Id,
            BatchNumber = batch.BatchNumber,
            BatchDate = batch.BatchDate,
            Status = batch.Status ?? "Draft",
            ApprovalStatus = batch.Status ?? "Draft"
        };
    }
}
