using Accounting.Application.PostingBatch.Commands;

namespace Accounting.Application.PostingBatch.Handlers
{
    public class ReversePostingBatchHandler(IRepository<Accounting.Domain.PostingBatch> repository)
        : IRequestHandler<ReversePostingBatchCommand>
    {
        public async Task Handle(ReversePostingBatchCommand request, CancellationToken cancellationToken)
        {
            var batch = await repository.GetByIdAsync(request.Id, cancellationToken);
            if (batch == null)
                throw new NotFoundException($"PostingBatch with Id {request.Id} not found");
            batch.Reverse(request.Reason);
            await repository.UpdateAsync(batch, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);
            // No return needed for Task
        }
    }
}
