using FSH.Framework.Core.Identity.Users.Abstractions;

namespace Accounting.Application.PostingBatches.Approve.v1;

public sealed class ApprovePostingBatchHandler(
    ICurrentUser currentUser,
    IRepository<PostingBatch> repository,
    ILogger<ApprovePostingBatchHandler> logger)
    : IRequestHandler<ApprovePostingBatchCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(ApprovePostingBatchCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        logger.LogInformation("Approving posting batch {BatchId}", request.Id);

        var batch = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (batch == null) throw new NotFoundException($"Posting batch with ID {request.Id} not found");

        var approverId = currentUser.GetUserId();
        var approverName = currentUser.GetUserName();
        
        batch.Approve(approverId, approverName);
        await repository.UpdateAsync(batch, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Posting batch {BatchNumber} approved by {ApproverId}", batch.BatchNumber, approverId);
        return batch.Id;
    }
}

