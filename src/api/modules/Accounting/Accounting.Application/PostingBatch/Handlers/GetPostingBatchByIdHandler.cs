using Accounting.Application.PostingBatch.Dtos;
using Accounting.Application.PostingBatch.Queries;

namespace Accounting.Application.PostingBatch.Handlers
{
    public class GetPostingBatchByIdHandler(IReadRepository<Accounting.Domain.PostingBatch> repository)
        : IRequestHandler<GetPostingBatchByIdQuery, PostingBatchDto>
    {
        public async Task<PostingBatchDto> Handle(GetPostingBatchByIdQuery request, CancellationToken cancellationToken)
        {
            var batch = await repository.GetByIdAsync(request.Id, cancellationToken);
            if (batch == null)
                throw new NotFoundException($"PostingBatch with Id {request.Id} not found");
            return new PostingBatchDto
            {
                Id = batch.Id,
                BatchNumber = batch.BatchNumber,
                BatchDate = batch.BatchDate,
                Status = batch.Status,
                Description = batch.Description,
                PeriodId = batch.PeriodId,
                ApprovalStatus = batch.ApprovalStatus,
                ApprovedBy = batch.ApprovedBy,
                ApprovedDate = batch.ApprovedDate,
                JournalEntryIds = batch.JournalEntries.Select(j => j.Id).ToList()
            };
        }
    }
}
