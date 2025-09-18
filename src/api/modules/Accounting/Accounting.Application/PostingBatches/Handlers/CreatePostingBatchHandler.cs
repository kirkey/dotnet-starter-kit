using Accounting.Application.PostingBatches.Commands;

namespace Accounting.Application.PostingBatches.Handlers;

public class CreatePostingBatchHandler(IRepository<PostingBatch> repository)
    : IRequestHandler<CreatePostingBatchCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreatePostingBatchCommand request, CancellationToken cancellationToken)
    {
        var batch = PostingBatch.Create(
            request.BatchNumber,
            request.BatchDate,
            request.Description,
            request.PeriodId);

        await repository.AddAsync(batch, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        return batch.Id;
    }
}