using Accounting.Application.PostingBatches.Queries;
using Accounting.Application.PostingBatches.Responses;

namespace Accounting.Application.PostingBatches.Handlers;

public class GetPostingBatchByIdHandler(IReadRepository<PostingBatch> repository)
    : IRequestHandler<GetPostingBatchByIdQuery, PostingBatchResponse>
{
    public async Task<PostingBatchResponse> Handle(GetPostingBatchByIdQuery request, CancellationToken cancellationToken)
    {
        var batch = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (batch == null)
            throw new NotFoundException($"PostingBatch with Id {request.Id} not found");
        return new PostingBatchResponse
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
