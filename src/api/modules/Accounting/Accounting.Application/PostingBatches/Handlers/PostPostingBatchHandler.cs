using Accounting.Application.PostingBatches.Commands;

namespace Accounting.Application.PostingBatches.Handlers;

public class PostingBatchHandler(IRepository<PostingBatch> repository)
    : IRequestHandler<PostingBatchCommand>
{
    public async Task Handle(PostingBatchCommand request, CancellationToken cancellationToken)
    {
        var batch = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = batch ?? throw new PostingBatchByIdNotFoundException(request.Id);
        batch.Post();
        await repository.UpdateAsync(batch, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
