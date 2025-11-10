using FSH.Framework.Core.Identity.Users.Abstractions;

namespace Accounting.Application.PostingBatches.Reject.v1;

/// <summary>
/// Handler for rejecting a posting batch.
/// The rejector is automatically determined from the current user session.
/// </summary>
public sealed class PostingBatchRejectHandler(
    ICurrentUser currentUser,
    [FromKeyedServices("accounting:postingBatches")] IRepository<PostingBatch> repository,
    ILogger<PostingBatchRejectHandler> logger)
    : IRequestHandler<PostingBatchRejectCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(PostingBatchRejectCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        logger.LogInformation("Rejecting posting batch {BatchId}", request.Id);

        var batch = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (batch == null)
            throw new NotFoundException($"Posting batch with ID {request.Id} not found");

        var rejectedBy = currentUser.GetUserName() ?? currentUser.GetUserId().ToString();
        batch.Reject(rejectedBy);
        
        await repository.UpdateAsync(batch, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Posting batch {BatchNumber} rejected by {RejectedBy}", batch.BatchNumber, rejectedBy);
        return batch.Id;
    }
}

