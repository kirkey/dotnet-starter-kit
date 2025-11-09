using Accounting.Application.PostingBatches.Commands;
using FSH.Framework.Core.Identity.Users.Abstractions;

namespace Accounting.Application.PostingBatches.Handlers;

/// <summary>
/// Handler for approving a posting batch.
/// The approver is automatically determined from the current user session.
/// </summary>
public class ApprovePostingBatchHandler(
    ICurrentUser currentUser,
    IRepository<PostingBatch> repository)
    : IRequestHandler<ApprovePostingBatchCommand>
{
    public async Task Handle(ApprovePostingBatchCommand request, CancellationToken cancellationToken)
    {
        var batch = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = batch ?? throw new PostingBatchByIdNotFoundException(request.Id);
        
        var approverId = currentUser.GetUserId();
        var approverName = currentUser.GetUserName();
        
        batch.Approve(approverId, approverName);
        await repository.UpdateAsync(batch, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
