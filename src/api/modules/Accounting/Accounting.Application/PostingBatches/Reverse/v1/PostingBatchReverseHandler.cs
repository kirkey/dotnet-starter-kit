namespace Accounting.Application.PostingBatches.Reverse.v1;

public sealed class PostingBatchReverseHandler(
    IRepository<PostingBatch> repository,
    ILogger<PostingBatchReverseHandler> logger)
    : IRequestHandler<PostingBatchReverseCommand, DefaultIdType>
{
    private readonly IRepository<PostingBatch> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<PostingBatchReverseHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(PostingBatchReverseCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Reversing posting batch {BatchId} - Reason: {Reason}", request.Id, request.Reason);

        var batch = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (batch == null) throw new NotFoundException($"Posting batch with ID {request.Id} not found");

        batch.Reverse(request.Reason);
        await _repository.UpdateAsync(batch, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Posting batch {BatchNumber} reversed successfully", batch.BatchNumber);
        return batch.Id;
    }
}
