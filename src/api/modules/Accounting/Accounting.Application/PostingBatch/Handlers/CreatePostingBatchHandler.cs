using MediatR;
using Accounting.Application.PostingBatch.Commands;
using FSH.Framework.Core.Persistence;

namespace Accounting.Application.PostingBatch.Handlers
{
    public class CreatePostingBatchHandler(IRepository<Accounting.Domain.PostingBatch> repository)
        : IRequestHandler<CreatePostingBatchCommand, DefaultIdType>
    {
        public async Task<DefaultIdType> Handle(CreatePostingBatchCommand request, CancellationToken cancellationToken)
        {
            var batch = Accounting.Domain.PostingBatch.Create(
                request.BatchNumber,
                request.BatchDate,
                request.Description,
                request.PeriodId);

            await repository.AddAsync(batch, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);
            return batch.Id;
        }
    }
}
