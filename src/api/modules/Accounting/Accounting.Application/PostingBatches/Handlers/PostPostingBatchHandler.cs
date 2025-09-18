using Accounting.Application.PostingBatches.Commands;

namespace Accounting.Application.PostingBatches.Handlers;

public class PostingBatchHandler(IRepository<PostingBatch> repository)
    : IRequestHandler<PostingBatchCommand>
{
    public async Task Handle(PostingBatchCommand request, CancellationToken cancellationToken)
    {
        var batch = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (batch == null)
            throw new NotFoundException($"PostingBatch with Id {request.Id} not found");
        batch.Post();
        await repository.UpdateAsync(batch, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}