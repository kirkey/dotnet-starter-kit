using Accounting.Application.PostingBatches.Commands;
using FSH.Framework.Core.Identity.Users.Abstractions;

namespace Accounting.Application.PostingBatches.Handlers;

public class PostingBatchHandler(
    IRepository<PostingBatch> repository,
    ICurrentUser currentUser)
    : IRequestHandler<PostingBatchCommand>
{
    public async Task Handle(PostingBatchCommand request, CancellationToken cancellationToken)
    {
        var batch = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = batch ?? throw new PostingBatchByIdNotFoundException(request.Id);
        
        var postedBy = currentUser.GetUserName() ?? currentUser.Name ?? "System";
        batch.Post(postedBy);
        await repository.UpdateAsync(batch, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
