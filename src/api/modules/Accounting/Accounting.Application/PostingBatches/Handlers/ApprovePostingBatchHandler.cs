using Accounting.Application.PostingBatches.Commands;
using Accounting.Domain.Entities;

namespace Accounting.Application.PostingBatches.Handlers;

public class ApprovePostingBatchHandler(IRepository<PostingBatch> repository)
    : IRequestHandler<ApprovePostingBatchCommand>
{
    public async Task Handle(ApprovePostingBatchCommand request, CancellationToken cancellationToken)
    {
        var batch = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = batch ?? throw new PostingBatchByIdNotFoundException(request.Id);
        batch.Approve(request.ApprovedBy);
        await repository.UpdateAsync(batch, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
