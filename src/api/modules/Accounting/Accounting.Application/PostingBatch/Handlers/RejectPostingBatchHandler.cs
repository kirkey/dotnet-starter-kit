using Accounting.Application.PostingBatch.Commands;

namespace Accounting.Application.PostingBatch.Handlers
{
    public class RejectPostingBatchHandler(IRepository<Accounting.Domain.PostingBatch> repository)
        : IRequestHandler<RejectPostingBatchCommand>
    {
        public async Task Handle(RejectPostingBatchCommand request, CancellationToken cancellationToken)
        {
            var batch = await repository.GetByIdAsync(request.Id, cancellationToken);
            if (batch == null)
                throw new NotFoundException($"PostingBatch with Id {request.Id} not found");
            batch.Reject(request.RejectedBy);
            await repository.UpdateAsync(batch, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);
        }
    }
}
