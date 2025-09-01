using Accounting.Domain;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.PostingBatch.Commands;
using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;

namespace Accounting.Application.PostingBatch.Handlers
{
    public class PostPostingBatchHandler(IRepository<Accounting.Domain.PostingBatch> repository)
        : IRequestHandler<PostPostingBatchCommand>
    {
        public async Task Handle(PostPostingBatchCommand request, CancellationToken cancellationToken)
        {
            var batch = await repository.GetByIdAsync(request.Id, cancellationToken);
            if (batch == null)
                throw new NotFoundException($"PostingBatch with Id {request.Id} not found");
            batch.Post();
            await repository.UpdateAsync(batch, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);
        }
    }
}
