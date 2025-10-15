using Accounting.Application.DebitMemos.Responses;

namespace Accounting.Application.DebitMemos.Search;

/// <summary>
/// Query for searching debit memos with pagination and filtering.
/// </summary>
public class SearchDebitMemosQuery : PaginationFilter, IRequest<PagedList<DebitMemoResponse>>
{
    /// <summary>
    /// Filter by memo number.
    /// </summary>
    public string? MemoNumber { get; set; }
    
    /// <summary>
    /// Filter by reference type (Customer or Vendor).
    /// </summary>
    public string? ReferenceType { get; set; }
    
    /// <summary>
    /// Filter by reference ID (customer or vendor ID).
    /// </summary>
    public DefaultIdType? ReferenceId { get; set; }
    
    /// <summary>
    /// Filter by status (Draft, Approved, Applied, Voided).
    /// </summary>
    public string? Status { get; set; }
    
    /// <summary>
    /// Filter by approval status (Pending, Approved, Rejected).
    /// </summary>
    public string? ApprovalStatus { get; set; }
    
    /// <summary>
    /// Filter by minimum amount.
    /// </summary>
    public decimal? AmountFrom { get; set; }
    
    /// <summary>
    /// Filter by maximum amount.
    /// </summary>
    public decimal? AmountTo { get; set; }
    
    /// <summary>
    /// Filter by minimum memo date.
    /// </summary>
    public DateTime? MemoDateFrom { get; set; }
    
    /// <summary>
    /// Filter by maximum memo date.
    /// </summary>
    public DateTime? MemoDateTo { get; set; }
    
    /// <summary>
    /// Filter to show only memos with unapplied amount.
    /// </summary>
    public bool? HasUnappliedAmount { get; set; }
}
