namespace Accounting.Application.PostingBatches.Approve.v1;

public sealed class PostingBatchApproveHandler(
    IRepository<PostingBatch> repository,
    ILogger<PostingBatchApproveHandler> logger)
    : IRequestHandler<PostingBatchApproveCommand, DefaultIdType>
{
    private readonly IRepository<PostingBatch> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<PostingBatchApproveHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(PostingBatchApproveCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Approving posting batch {BatchId}", request.Id);

        var batch = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (batch == null) throw new NotFoundException($"Posting batch with ID {request.Id} not found");

        batch.Approve(request.ApprovedBy);
        await _repository.UpdateAsync(batch, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Posting batch {BatchNumber} approved by {ApprovedBy}", batch.BatchNumber, request.ApprovedBy);
        return batch.Id;
    }
}

