using FSH.Framework.Core.Identity.Users.Abstractions;

namespace Accounting.Application.PostingBatches.Reverse.v1;

/// <summary>
/// Handler for reversing a posted batch.
/// The reverser is automatically determined from the current user session.
/// </summary>
public sealed class ReversePostingBatchHandler(
    ICurrentUser currentUser,
    [FromKeyedServices("accounting:posting-batches")] IRepository<PostingBatch> repository,
    ILogger<ReversePostingBatchHandler> logger)
    : IRequestHandler<ReversePostingBatchCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(ReversePostingBatchCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        logger.LogInformation("Reversing posting batch {BatchId} - Reason: {Reason}", request.Id, request.Reason);

        var batch = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (batch == null)
            throw new NotFoundException($"Posting batch with ID {request.Id} not found");

        var reversedBy = currentUser.GetUserName() ?? currentUser.GetUserId().ToString();
        batch.Reverse(reversedBy, request.Reason);
        
        await repository.UpdateAsync(batch, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Posting batch {BatchNumber} reversed successfully by {User}", batch.BatchNumber, reversedBy);
        return batch.Id;
    }
}
