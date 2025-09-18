using Accounting.Application.PostingBatches.Commands;

namespace Accounting.Application.PostingBatches.Handlers;

public class ApprovePostingBatchHandler(IRepository<PostingBatch> repository)
    : IRequestHandler<ApprovePostingBatchCommand>
{
    public async Task Handle(ApprovePostingBatchCommand request, CancellationToken cancellationToken)
    {
        var batch = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (batch == null)
            throw new NotFoundException($"PostingBatch with Id {request.Id} not found");
        batch.Approve(request.ApprovedBy);
        await repository.UpdateAsync(batch, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}