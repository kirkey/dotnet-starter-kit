using Accounting.Application.PostingBatches.Responses;

namespace Accounting.Application.PostingBatches.Queries;

/// <summary>
/// Query to search posting batches with filtering and pagination.
/// </summary>
public class SearchPostingBatchesQuery : PaginationFilter, IRequest<PagedList<PostingBatchResponse>>
{
    /// <summary>
    /// Gets or sets the batch number to filter by (partial match).
    /// </summary>
    public string? BatchNumber { get; set; }

    /// <summary>
    /// Gets or sets the status to filter by.
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Gets or sets the approval status to filter by.
    /// </summary>
    public string? ApprovalStatus { get; set; }
}
