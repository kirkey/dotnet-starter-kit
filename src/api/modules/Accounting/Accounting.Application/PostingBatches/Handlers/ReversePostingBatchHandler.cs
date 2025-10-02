using Accounting.Application.PostingBatches.Commands;
using Accounting.Domain.Entities;

namespace Accounting.Application.PostingBatches.Handlers;

public class ReversePostingBatchHandler(IRepository<PostingBatch> repository)
    : IRequestHandler<ReversePostingBatchCommand>
{
    public async Task Handle(ReversePostingBatchCommand request, CancellationToken cancellationToken)
    {
        var batch = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = batch ?? throw new PostingBatchByIdNotFoundException(request.Id);
        batch.Reverse(request.Reason);
        await repository.UpdateAsync(batch, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        // No return needed for Task
    }
}
