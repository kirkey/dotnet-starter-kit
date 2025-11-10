namespace Accounting.Application.PostingBatches.Update.v1;

/// <summary>
/// Handler for updating a posting batch.
/// Only draft/pending batches can be updated.
/// </summary>
public sealed class UpdatePostingBatchHandler(
    [FromKeyedServices("accounting:postingBatches")] IRepository<PostingBatch> repository,
    ILogger<UpdatePostingBatchHandler> logger)
    : IRequestHandler<UpdatePostingBatchCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(UpdatePostingBatchCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        logger.LogInformation("Updating posting batch {BatchId}", request.Id);

        var batch = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (batch == null)
            throw new NotFoundException($"Posting batch with ID {request.Id} not found");

        if (batch.Status != "Pending" && batch.Status != "Draft")
            throw new InvalidOperationException($"Cannot update batch with status {batch.Status}. Only Pending or Draft batches can be updated.");

        batch.Update(request.BatchDate, request.Description, request.PeriodId);

        await repository.UpdateAsync(batch, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Posting batch {BatchNumber} updated successfully", batch.BatchNumber);
        return batch.Id;
    }
}

