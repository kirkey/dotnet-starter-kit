using FSH.Framework.Core.Identity.Users.Abstractions;

namespace Accounting.Application.PostingBatches.Post.v1;

/// <summary>
/// Handler for posting a batch to the general ledger.
/// The poster is automatically determined from the current user session.
/// </summary>
public sealed class PostingBatchPostHandler(
    ICurrentUser currentUser,
    [FromKeyedServices("accounting:posting-batches")] IRepository<PostingBatch> repository,
    ILogger<PostingBatchPostHandler> logger)
    : IRequestHandler<PostingBatchPostCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(PostingBatchPostCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        logger.LogInformation("Posting batch {BatchId}", request.Id);

        var batch = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (batch == null)
            throw new NotFoundException($"Posting batch with ID {request.Id} not found");

        var postedBy = currentUser.GetUserName() ?? currentUser.GetUserId().ToString();
        batch.Post(postedBy);
        
        await repository.UpdateAsync(batch, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Posting batch {BatchNumber} posted successfully by {User}", batch.BatchNumber, postedBy);
        return batch.Id;
    }
}

