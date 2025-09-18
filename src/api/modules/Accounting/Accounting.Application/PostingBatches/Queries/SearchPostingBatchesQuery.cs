using Accounting.Application.PostingBatches.Dtos;

namespace Accounting.Application.PostingBatches.Queries;

public class SearchPostingBatchesQuery : IRequest<List<PostingBatchDto>>
{
    public string? BatchNumber { get; set; }
    public string? Status { get; set; }
    public string? ApprovalStatus { get; set; }
    public int? Skip { get; set; }
    public int? Take { get; set; }
}