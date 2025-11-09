using Accounting.Application.PostingBatches.Commands;
using FSH.Framework.Core.Identity.Users.Abstractions;

namespace Accounting.Application.PostingBatches.Handlers;

public class ReversePostingBatchHandler(
    IRepository<PostingBatch> repository,
    ICurrentUser currentUser)
    : IRequestHandler<ReversePostingBatchCommand>
{
    public async Task Handle(ReversePostingBatchCommand request, CancellationToken cancellationToken)
    {
        var batch = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = batch ?? throw new PostingBatchByIdNotFoundException(request.Id);
        
        var reversedBy = currentUser.GetUserName() ?? currentUser.Name ?? "System";
        batch.Reverse(reversedBy, request.Reason);
        await repository.UpdateAsync(batch, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
