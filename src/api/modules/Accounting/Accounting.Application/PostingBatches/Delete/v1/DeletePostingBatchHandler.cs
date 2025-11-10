namespace Accounting.Application.PostingBatches.Delete.v1;

/// <summary>
/// Command to delete a posting batch.
/// Only draft/pending batches with no journal entries can be deleted.
/// </summary>
public sealed record DeletePostingBatchCommand(DefaultIdType Id) : IRequest<DefaultIdType>;

/// <summary>
/// Handler for deleting a posting batch.
/// Only draft/pending batches with no journal entries can be deleted.
/// </summary>
public sealed class DeletePostingBatchHandler(
    [FromKeyedServices("accounting:posting-batches")] IRepository<PostingBatch> repository,
    ILogger<DeletePostingBatchHandler> logger)
    : IRequestHandler<DeletePostingBatchCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(DeletePostingBatchCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        logger.LogInformation("Deleting posting batch {BatchId}", request.Id);

        var batch = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (batch == null)
            throw new NotFoundException($"Posting batch with ID {request.Id} not found");

        if (batch.Status != "Pending" && batch.Status != "Draft")
            throw new InvalidOperationException($"Cannot delete batch with status {batch.Status}. Only Pending or Draft batches can be deleted.");

        if (batch.JournalEntries.Any())
            throw new InvalidOperationException("Cannot delete batch with journal entries. Remove all entries first.");

        await repository.DeleteAsync(batch, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Posting batch {BatchNumber} deleted successfully", batch.BatchNumber);
        return request.Id;
    }
}

