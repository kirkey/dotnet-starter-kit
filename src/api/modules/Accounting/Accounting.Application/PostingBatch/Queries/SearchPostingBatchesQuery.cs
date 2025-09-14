using Accounting.Application.PostingBatch.Dtos;

namespace Accounting.Application.PostingBatch.Queries
{
    public class SearchPostingBatchesQuery : IRequest<List<PostingBatchDto>>
    {
        public string? BatchNumber { get; set; }
        public string? Status { get; set; }
        public string? ApprovalStatus { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
    }
}

