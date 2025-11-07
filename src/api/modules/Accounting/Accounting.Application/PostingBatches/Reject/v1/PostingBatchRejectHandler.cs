namespace Accounting.Application.PostingBatches.Reject.v1;

public sealed class PostingBatchRejectHandler(
    IRepository<PostingBatch> repository,
    ILogger<PostingBatchRejectHandler> logger)
    : IRequestHandler<PostingBatchRejectCommand, DefaultIdType>
{
    private readonly IRepository<PostingBatch> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<PostingBatchRejectHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(PostingBatchRejectCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Rejecting posting batch {BatchId}", request.Id);

        var batch = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (batch == null) throw new NotFoundException($"Posting batch with ID {request.Id} not found");

        batch.Reject(request.RejectedBy);
        await _repository.UpdateAsync(batch, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Posting batch {BatchNumber} rejected by {RejectedBy}", batch.BatchNumber, request.RejectedBy);
        return batch.Id;
    }
}

