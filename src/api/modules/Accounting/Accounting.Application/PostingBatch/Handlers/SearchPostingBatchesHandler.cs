using MediatR;
using Accounting.Application.PostingBatch.Queries;
using Accounting.Application.PostingBatch.Dtos;
using FSH.Framework.Core.Persistence;

namespace Accounting.Application.PostingBatch.Handlers
{
    public class SearchPostingBatchesHandler(IReadRepository<Domain.PostingBatch> repository)
        : IRequestHandler<SearchPostingBatchesQuery, List<PostingBatchDto>>
    {
        public async Task<List<PostingBatchDto>> Handle(SearchPostingBatchesQuery request, CancellationToken cancellationToken)
        {
            var query = (await repository.ListAsync(cancellationToken)).AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.BatchNumber))
                query = query.Where(x => x.BatchNumber.Contains(request.BatchNumber));
            if (!string.IsNullOrWhiteSpace(request.Status))
                query = query.Where(x => x.Status == request.Status);
            if (!string.IsNullOrWhiteSpace(request.ApprovalStatus))
                query = query.Where(x => x.ApprovalStatus == request.ApprovalStatus);
            if (request.Skip.HasValue)
                query = query.Skip(request.Skip.Value);
            if (request.Take.HasValue)
                query = query.Take(request.Take.Value);

            return query.Select(batch => new PostingBatchDto
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
            }).ToList();
        }
    }
}

